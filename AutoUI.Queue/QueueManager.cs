using AutoUI.Common;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Queue
{
    public class QueueManager
    {
        public QueueManager()
        {
            ctx = new QueueDbContext();
        }
        public string ExecutorIP = "127.0.0.1";
        public int ExecutorPort = 8888;
        static QueueDbContext ctx;

        public async void Run()
        {
            Console.WriteLine($"Starting queue manager {ExecutorIP}:{ExecutorPort}...");

            Thread th = new(async () =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(5000);
                        var cands = ctx.Runs.Where(z => z.Status == RunStatus.NotStarted).ToArray();

                        if (cands.Length == 0)
                            continue;

                        var toRun = cands.First();

                        Console.WriteLine($"Starting new run #{toRun.Id} {toRun.Name} ({toRun.Version})...");

                        using TcpClient client = new TcpClient();
                        await client.ConnectAsync(ExecutorIP, ExecutorPort);
                        var stream = client.GetStream();
                        var wr = new StreamWriter(stream);
                        var rdr = new StreamReader(stream);

                        var doc = XDocument.Parse(toRun.Xml);
                        if (!doc.Descendants("testXmlPath").Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("testXmlPath attribute not found");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        var xmlPath = doc.Descendants("testXmlPath").First().Value;
                        TestSet set = null;
                        if (xmlPath.ToLower().EndsWith(".azip"))
                        {
                            set = TestSet.LoadFromAZip(xmlPath);

                            var s1 = $"TEST_SET_AZIP={Convert.ToBase64String(File.ReadAllBytes(xmlPath))}";

                            await wr.WriteLineAsync(s1);
                        }
                        else
                        {
                            var testsXml = XDocument.Load(xmlPath);
                            set = new TestSet(testsXml.Root);
                            var s1 = $"TEST_SET={Convert.ToBase64String(Encoding.Default.GetBytes(set.ToXml().ToString()))}";

                            await wr.WriteLineAsync(s1);
                        }


                        await wr.FlushAsync();
                        var res = await rdr.ReadLineAsync();

                        toRun.Status = RunStatus.InProgress;
                        ctx.SaveChanges();
                        try
                        {                            
                            await wr.WriteLineAsync($"START_TEST_SET");
                            await wr.FlushAsync();
                            await rdr.ReadLineAsync();

                            RunStatus status = RunStatus.NotStarted;
                            for (int i = 0; i < set.Tests.Count; i++)
                            {
                                var test = set.Tests[i];
                                var testIdx = i;
                                status = await MakeRun(i, set, toRun, wr, rdr);
                                if (status != RunStatus.Succeed)                                
                                    break;                                
                            }
                            toRun.Status = status;
                            ctx.SaveChanges();
                                                        
                            await wr.WriteLineAsync($"FINISH_TEST_SET");
                            await wr.FlushAsync();
                            await rdr.ReadLineAsync();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Run succeed #{toRun.Id}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception ex)
                        {
                            toRun.Status = RunStatus.Failed;
                            toRun.ResultDescription = ex.Message;
                            ctx.SaveChanges();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Run failed #{toRun.Id}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        client.Close();
                        Console.WriteLine($"Finished run #{toRun.Id} ");

                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private async static Task<RunStatus> MakeRun(int testIdx, TestSet set, Run toRun, StreamWriter wr, StreamReader rdr)
        {
            var test = set.Tests[testIdx];
            var tri = new TestRunItem() { Parent = toRun, Name = test.Name, Timestamp = DateTime.UtcNow };
            tri.Status = RunStatus.InProgress;
            toRun.Tests.Add(tri);
            ctx.SaveChanges();
            StringBuilder sb = new StringBuilder();
            var doc = XDocument.Parse(toRun.Xml);
            foreach (var pitem in doc.Descendants("param"))
            {
                sb.Append(pitem.Attribute("name").Value);
                sb.Append("=");
                sb.Append($"{pitem.Value} ");
            }

            Stopwatch sw = Stopwatch.StartNew();
            var ret = await RemoteRunner.RunRemotely(wr, rdr, testIdx, sb.ToString());
            sw.Stop();

            tri.Duration = (int)sw.ElapsedMilliseconds;
            tri.Timestamp = DateTime.UtcNow;
            if (ret.State == TestStateEnum.Failed)
            {
                tri.XmlOutput = ret.ToXml().ToString();
                tri.Status = RunStatus.Failed;
                ctx.SaveChanges();
                return RunStatus.Failed;
            }

            tri.Status = RunStatus.Succeed;
            ctx.SaveChanges();

            return RunStatus.Succeed;
        }
    }
}
