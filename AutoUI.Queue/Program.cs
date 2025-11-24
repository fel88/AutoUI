using AutoUI.Common;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoUI.Queue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AutoUI.Queue starting..");
            ctx = new QueueDbContext();
            int port = 8888;
            if (!args.Any(z => z.StartsWith("--eip=")))
            {
                Usage();
                return;
            }
            var parsedArgs = args.Select(ParseArg);

            if (parsedArgs.Any(z => z.Item1 == "--port"))
                port = int.Parse(parsedArgs.First(z => z.Item1 == "--port").Item2);

            if (parsedArgs.Any(z => z.Item1 == "--eport"))
                ExecutorPort = int.Parse(parsedArgs.First(z => z.Item1 == "--eport").Item2);

            if (parsedArgs.Any(z => z.Item1 == "--eip"))
                ExecutorIP = parsedArgs.First(z => z.Item1 == "--eip").Item2;

            //run queue manager
            QueueManager();

            //run server
            Server server = new Server();
            server.Init(port);
            server.th.Join();
        }

        static QueueDbContext ctx;

        private async static void QueueManager()
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(ExecutorIP, ExecutorPort);
            var stream = client.GetStream();
            var wr = new StreamWriter(stream);
            var rdr = new StreamReader(stream);

            Thread th = new(async () =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    var cands = ctx.Runs.Where(z => z.Status == RunStatus.NotStarted).ToArray();

                    if (cands.Length == 0)
                        continue;

                    var toRun = cands.First();
                    var xmlPath = XDocument.Parse(toRun.Xml).Descendants("testXmlPath").First().Value;
                    var testsXml = XDocument.Load(xmlPath);
                    var set = new TestSet(testsXml.Root);

                    var s1 = $"TEST_SET={Convert.ToBase64String(Encoding.Default.GetBytes(set.ToXml().ToString()))}";

                    await wr.WriteLineAsync(s1);
                    await wr.FlushAsync();
                    var res = await rdr.ReadLineAsync();


                    toRun.Status = RunStatus.InProgress;
                    ctx.SaveChanges();

                    var status = await MakeRun(set, toRun, wr, rdr);
                    toRun.Status = status;
                    ctx.SaveChanges();
                }
            });
            th.IsBackground = true;
            th.Start();

        }

        static string ExecutorIP;
        static int ExecutorPort = 8888;

        private async static Task<RunStatus> MakeRun(TestSet set, Run toRun, StreamWriter wr, StreamReader rdr)
        {
            foreach (var item in set.Tests)
            {
                var testIdx = set.Tests.IndexOf(item);

                var tri = new TestRunItem() { Parent = toRun, Name = item.Name };
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
                var ret = await RunRemotely(wr, rdr, sb.ToString(), testIdx);
                item.State = ret.Item2;

                if (ret.Item2 == TestStateEnum.Failed)
                {
                    tri.Status = RunStatus.Failed;
                    ctx.SaveChanges();
                    return RunStatus.Failed;

                }
                tri.Status = RunStatus.Succeed;
                ctx.SaveChanges();
            }

            return RunStatus.Succeed;
        }

        private static async Task<(AutoTestRunContext, TestStateEnum)> RunRemotely(StreamWriter wr, StreamReader rdr, string testParams, int testIdx)
        {
            await wr.WriteLineAsync($"RUN_TEST={testIdx};{testParams}");
            await wr.FlushAsync();
            var res = await rdr.ReadLineAsync();

            var spl = res.Split(new[] { "RESULT", "=" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            var tt = Enum.Parse<TestStateEnum>(spl[0]);
            return (new AutoTestRunContext(), tt);
        }
        static (string, string) ParseArg(string str)
        {
            var spl = str.Split("=");
            var ret = spl[1];
            return (spl[0], ret);
        }

        private static void Usage()
        {
            System.Console.WriteLine("usage: --xml=<path> --port=.. --eport=.. --eip=..");
        }
    }

}
