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

namespace AutoUI.Queue
{
    public class Server : TcpRoutine
    {
        public void Init(int port)
        {
            InitTcp(IPAddress.Any, port, ThreadProcessor, () => new UserInfo());

        }


        public void ThreadProcessor(NetworkStream stream, object obj)
        {
            var cinf = obj as ConnectionInfo;
            var uinfo = cinf.Tag as UserInfo;
            StreamReader reader = new StreamReader(stream);
            StreamWriter wrt2 = new StreamWriter(stream);
            QueueDbContext ctx = new QueueDbContext();
            while (true)
            {
                try
                {
                    var line = reader.ReadLine();

                    if (line.StartsWith("QUEUE_RUN"))
                    {
                        //1.parse xml
                        var ln = line.Substring("QUEUE_RUN".Length + 1);
                        var spl = Encoding.UTF8.GetString(Convert.FromBase64String(ln));
                        var doc = XDocument.Parse(spl);

                        var newRun = new Run()
                        {
                            Name = doc.Root.Element("name").Value,
                            Version = doc.Root.Element("version").Value,
                            Xml = spl
                        };
                        ctx.Runs.Add(newRun);
                        ctx.SaveChanges();
                        

                        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} QUEUE_RUN={spl}");
                        
                        wrt2.WriteLine($"ID={newRun}");
                        wrt2.Flush();                        

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
