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
        public override void NewClient()
        {
            sendAllClientUpdates();
        }
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

                    if (line.StartsWith("INIT"))
                    {
                        var ind = line.IndexOf("=");
                        var msg = line.Substring(ind + 1);
                        if (string.IsNullOrWhiteSpace(msg))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[init] reject empty user name ");
                            Console.ForegroundColor = ConsoleColor.White;
                            //reject connection
                            stream.Close();
                            break;
                        }
                        uinfo.Name = msg;

                        if (streams.Where(z => z.Tag != uinfo).Select(z => z.Tag as UserInfo).Any(z => z.Name == msg))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[init] reject duplicate user name = " + msg);
                            Console.ForegroundColor = ConsoleColor.White;
                            //reject connection
                            stream.Close();
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("user init = " + msg);
                        Console.ForegroundColor = ConsoleColor.White;
                        NewClient();

                    }
                    else if (line.StartsWith("MSG"))
                    {

                        var ind = line.IndexOf("=");
                        var msg = line.Substring(ind + 1);
                        var spl = msg.Split(';').ToArray();
                        var target = spl[1];

                        var bs64 = Convert.FromBase64String(spl[0]);
                        var str = Encoding.UTF8.GetString(bs64);


                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<?xml version=\"1.0\"?>");
                        sb.AppendLine("<root>");
                        sb.AppendLine(string.Format("<message user=\"{0}\" target=\"{1}\">", uinfo.Name, target));
                        sb.AppendFormat("<![CDATA[{0}]]>", str);
                        sb.AppendLine(string.Format("</message>", uinfo.Name, str));
                        sb.AppendLine("</root>");

                        var estr = sb.ToString();


                        var bt = Encoding.UTF8.GetBytes(estr);

                        var ree = Convert.ToBase64String(bt);

                        this.SendTo("MSG=" + ree, target);
                    }
                    else if (line.StartsWith("TYPING"))
                    {

                        var ind = line.IndexOf("=");
                        var msg = line.Substring(ind + 1);
                        var spl = msg.Split(';').ToArray();
                        var target = spl[1];

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<?xml version=\"1.0\"?>");
                        sb.AppendLine("<root>");
                        sb.AppendLine(string.Format("<message user=\"{0}\" target=\"{1}\">", uinfo.Name, target));
                        sb.AppendLine("</message>");
                        sb.AppendLine("</root>");

                        var estr = sb.ToString();


                        var bt = Encoding.UTF8.GetBytes(estr);

                        var ree = Convert.ToBase64String(bt);

                        this.SendTo("TYPING=" + ree, target);
                    }
                    else if (line.StartsWith("CLIENTS"))
                    {
                        sendClientsList(wrt2);
                    }
                    else if (line.StartsWith("ACK"))//file download ack
                    {
                        //1.parse xml
                        var ln = line.Substring("ACK".Length + 1);

                        var bs64 = Convert.FromBase64String(ln);

                        var str = Encoding.UTF8.GetString(bs64);
                        var doc = XDocument.Parse(str);
                        var w = doc.Descendants("ack").First();
                        var targ = w.Attribute("target").Value;
                        var cxstr = this.streams.First(z => (z.Tag as UserInfo).Name == targ);
                        var wr = new StreamWriter(cxstr.Stream);
                        //2. retranslate to specific client web request
                        wr.WriteLine(line);
                        wr.Flush();


                        //server.SendAll(line);
                    }
                    else if (line.StartsWith("TEST_SET"))
                    {
                        //1.parse xml
                        var ln = line.Substring("TEST_SET".Length + 1);

                        var bs64 = Convert.FromBase64String(ln);

                        var str = Encoding.UTF8.GetString(bs64);
                        var doc = XDocument.Parse(str);

                        CurrentSet = new TestSet(doc.Root);
                        Console.WriteLine("[TEST_SET] recieved");
                        wrt2.WriteLine($"OK");
                        wrt2.Flush();
                    }
                    else if (line.StartsWith("TEST"))
                    {
                        //1.parse xml
                        var ln = line.Substring("TEST".Length + 1);

                        var testIdx = int.Parse(ln);
                        var item = CurrentSet.Tests[testIdx];
                        Console.WriteLine($"[TEST #{testIdx}] starting...");
                        try
                        {
                            var res = item.Run();
                            int cc = 0;
                            foreach (var sub in res.SubTests)
                            {
                                var res1 = sub.Run();
                                cc++;
                            }

                            if (item.UseEmitter)
                                item.State = TestStateEnum.Emitter;

                            wrt2.WriteLine($"RESULT={item.State}");
                            wrt2.Flush();
                            Console.WriteLine($"[TEST #{testIdx}] finished.");

                        }
                        catch (Exception ex)
                        {
                            wrt2.WriteLine($"RESULT={TestStateEnum.Exception}");
                            wrt2.Flush();
                            Console.WriteLine($"[TEST #{testIdx}] exception: {ex.Message}");
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
                    sendAllClientUpdates();
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
                    sendAllClientUpdates();
                    break;
                    //TcpRoutine.ErrorSend(stream);
                }
            }
        }

        void sendAllClientUpdates()
        {
            var estr = getClientsXml();
            var bt = Encoding.UTF8.GetBytes(estr);
            var ree = Convert.ToBase64String(bt);
            var ss = "CLIENTS=" + ree;

            SendAll(ss);
        }

        public string getClientsXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<root>");
            foreach (var connectionInfo in this.streams)
            {
                var uin = connectionInfo.Tag as UserInfo;
                sb.AppendLine(string.Format("<client name=\"{0}\" />", uin.Name));
            }
            sb.AppendLine("</root>");
            return sb.ToString();
        }
        private void sendClientsList(StreamWriter wrt2)
        {
            var estr = getClientsXml();
            var bt = Encoding.UTF8.GetBytes(estr);
            var ree = Convert.ToBase64String(bt);
            wrt2.WriteLine("CLIENTS=" + ree);
            wrt2.Flush();
        }
    }
}
