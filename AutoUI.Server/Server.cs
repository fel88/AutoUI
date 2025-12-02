using AutoUI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Server
{
    public class Server : TcpRoutine
    {
        public void Init(int port)
        {
            InitTcp(IPAddress.Any, port, ThreadProcessor, () => new UserInfo());

        }

        static TestSet CurrentSet;
        static IAutoTest CurrentTest;

        public void ThreadProcessor(NetworkStream stream, object obj)
        {
            var cinf = obj as ConnectionInfo;
            var uinfo = cinf.Tag as UserInfo;
            StreamReader reader = new StreamReader(stream);
            StreamWriter wrt2 = new StreamWriter(stream);

            while (true)
            {
                try
                {
                    var line = reader.ReadLine();

                    if (line.StartsWith("TEST_SET_AZIP"))
                    {
                        var ln = line.Substring("TEST_SET_AZIP".Length + 1);

                        var bs64 = Convert.FromBase64String(ln);
                        using MemoryStream ms = new MemoryStream(bs64);

                        CurrentSet = TestSet.LoadFromAZipStream(ms);
                        Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} [TEST_SET_AZIP] recieved");
                        wrt2.WriteLine($"OK");
                        wrt2.Flush();
                    }
                    else if (line.StartsWith("TEST_SET"))
                    {
                        var ln = line.Substring("TEST_SET".Length + 1);

                        var bs64 = Convert.FromBase64String(ln);

                        var str = Encoding.UTF8.GetString(bs64);
                        var doc = XDocument.Parse(str);

                        CurrentSet = new TestSet(doc.Root);
                        Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} [TEST_SET] recieved");
                        wrt2.WriteLine($"OK");
                        wrt2.Flush();
                    }
                    else if (line.StartsWith("LOCAL_TEST_SET"))
                    {
                        //1.parse xml
                        var ln = line.Substring("LOCAL_TEST_SET".Length + 1);

                        var bs64 = Convert.FromBase64String(ln);

                        var str = Encoding.UTF8.GetString(bs64);
                        var doc = XDocument.Load(str);

                        CurrentSet = new TestSet(doc.Root);
                        Console.WriteLine($"[LOCAL_TEST_SET] recieved ({str})");
                        wrt2.WriteLine($"OK");
                        wrt2.Flush();
                    }
                    else if (line.StartsWith("TEST_ITEM"))
                    {
                        //1.parse xml
                        var ln = line.Substring("TEST_ITEM".Length + 1);
                        var testIdx = int.Parse(ln);
                        var subTest = CurrentTest.CurrentCodeSection.Items[testIdx];
                        var ctx = new AutoTestRunContext(CurrentTest);
                        var result = subTest.Process(ctx);

                        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} RESULT={result}");
                        wrt2.WriteLine($"RESULT={result}");
                        wrt2.Flush();
                        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} [TEST_ITEM #{testIdx}] finished.");

                    }
                    else if (line.StartsWith("SET_TEST"))
                    {
                        var ln = line.Substring("SET_TEST".Length + 1);

                        var testIdx = int.Parse(ln);
                        var item = CurrentSet.Tests[testIdx];
                        Console.WriteLine($"[SET_TEST #{testIdx}]");
                        wrt2.WriteLine($"OK");
                        wrt2.Flush();
                    }
                    else if (line.StartsWith("RUN_TEST"))
                    {
                        var ln = line.Substring("RUN_TEST".Length + 1);
                        var spl = ln.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        var testIdx = int.Parse(spl[0]);

                        var item = CurrentSet.Tests[testIdx];
                        if (spl.Length > 1)
                        {
                            var testParams = spl[1];
                            var pp = testParams.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();


                            foreach (var p in pp)
                            {
                                var spl1 = ln.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                item.Data[spl1[0]] = spl1[1];
                            }
                        }

                        Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} [RUN_TEST #{testIdx}] starting...");
                        try
                        {

                            item.Reset();
                            var res = item.Run();
                            int cc = 0;
                            foreach (var sub in res.SubTests)
                            {
                                var res1 = sub.Run();
                                cc++;
                            }

                            Console.WriteLine($"RESULT {res.State}");
                            if (res.WrongState != null)
                                Console.WriteLine($"WrongState {res.WrongState.Name}: {res.WrongState.ToString()}");

                            Console.WriteLine($"CodePointer = {res.CodePointer} / {item.CurrentCodeSection.Items.Count}");

                            wrt2.WriteLine($"RESULT={Convert.ToBase64String(Encoding.UTF8.GetBytes(res.ToXml().ToString()))}");
                            wrt2.Flush();
                            Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} [RUN_TEST #{testIdx}] finished.");

                        }
                        catch (Exception ex)
                        {
                            wrt2.WriteLine($"RESULT={TestStateEnum.Exception}");
                            wrt2.Flush();
                            Console.WriteLine($"[RUN_TEST #{testIdx}] exception: {ex.Message}");
                        }

                    }
                    else if (line.StartsWith("FILE"))
                    {
                        //1.parse xml
                        var ln = line.Substring("FILE".Length + 1);

                        var bs64 = Convert.FromBase64String(ln);

                        var str = Encoding.UTF8.GetString(bs64);
                        var doc = XDocument.Parse(str);

                        var w = doc.Descendants("file").First();
                        var targ = w.Attribute("target").Value;

                        {
                            var cxstr = this.streams.First(z => (z.Tag as UserInfo).Name == targ);
                            var wr = new StreamWriter(cxstr.Stream);
                            //2. retranslate to specific client web request
                            wr.WriteLine(line);
                            wr.Flush();

                            //server.SendAll(line);
                        }
                    }
                }
                catch (IOException iex)
                {
                    lock (streams)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine((cinf.Tag as UserInfo).Name + " - removed");
                        Console.ForegroundColor = ConsoleColor.White;
                        streams.Remove(cinf);
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    lock (streams)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine((cinf.Tag as UserInfo).Name + " - removed");
                        Console.ForegroundColor = ConsoleColor.White;
                        streams.Remove(cinf);
                    }

                    break;
                    //TcpRoutine.ErrorSend(stream);
                }
            }
        }



    }
}
