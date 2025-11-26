using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace AutoUI.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AutoUI.Client");
            if (!args.Any(z => z.StartsWith("--ip=")) || !args.Any(z => z.StartsWith("--testXmlPath="))
                || !args.Any(z => z.StartsWith("--name="))
                || !args.Any(z => z.StartsWith("--version=")))
            {
                Usage();
                return;
            }
            var parsedArgs = args.Select(ParseArg).ToArray();

            if (parsedArgs.Any(z => z.Item1.StartsWith("--debugger")))
                Debugger.Launch();

            string testXmlPath = "";
            string name = "";
            string version = "";

            if (parsedArgs.Any(z => z.Item1 == "--testXmlPath"))
                testXmlPath = parsedArgs.First(z => z.Item1 == "--testXmlPath").Item2;

            if (parsedArgs.Any(z => z.Item1 == "--name"))
                name = parsedArgs.First(z => z.Item1 == "--name").Item2;

            if (parsedArgs.Any(z => z.Item1 == "--version"))
                version = parsedArgs.First(z => z.Item1 == "--version").Item2;



            XDocument doc = new XDocument();
            doc.Add(new XElement("root"));
            doc.Root.Add(new XElement("testXmlPath", new XCData(testXmlPath)));
            doc.Root.Add(new XElement("version", version));
            doc.Root.Add(new XElement("name", name));

            foreach (var item in parsedArgs.Where(z => z.Item1.StartsWith("--param:")))
            {
                var spl = item.Item1.Replace("--param:", "");
                var pp = new XElement("param", new XCData(item.Item2));
                pp.Add(new XAttribute("name", spl));
                doc.Root.Add(pp);
            }

            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(doc.ToString()));

            if (parsedArgs.Any(z => z.Item1 == "--port"))
                port = int.Parse(parsedArgs.First(z => z.Item1 == "--port").Item2);

            if (parsedArgs.Any(z => z.Item1 == "--ip"))
                ip = parsedArgs.First(z => z.Item1 == "--ip").Item2;

            try
            {
                TcpClient tcpClient = new TcpClient(ip, port);
                var stream = tcpClient.GetStream();
                using var sw = new StreamWriter(stream);
                using var sr = new StreamReader(stream);
                sw.WriteLine($"QUEUE_RUN={b64}");
                sw.Flush();
                Console.WriteLine(sr.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static int port = 8889;
        static string ip = "127.0.0.1";

        static (string, string) ParseArg(string str)
        {
            var spl = str.Split("=");
            if (spl.Length > 1)
            {
                var ret = spl[1];
                return (spl[0], ret);
            }
            return (spl[0], string.Empty);
        }

        private static void Usage()
        {
            System.Console.WriteLine("usage: --name=.. --ip=.. --port=.. --version=.. --testXmlPath=.. --param:<param_name>=..");
        }
    }
}
