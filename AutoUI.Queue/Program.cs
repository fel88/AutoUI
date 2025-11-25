using System.Windows.Forms;

namespace AutoUI.Queue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AutoUI.Queue starting..");

            int port = 8889;
            QueueManager qm = new QueueManager();
            //if (!args.Any(z => z.StartsWith("--eip=")))
            {
                //    Usage();
                //return;
            }
            var parsedArgs = args.Select(ParseArg);

            if (parsedArgs.Any(z => z.Item1 == "--port"))
                port = int.Parse(parsedArgs.First(z => z.Item1 == "--port").Item2);

            if (parsedArgs.Any(z => z.Item1 == "--eport"))
                qm.ExecutorPort = int.Parse(parsedArgs.First(z => z.Item1 == "--eport").Item2);

            if (parsedArgs.Any(z => z.Item1 == "--eip"))
                qm.ExecutorIP = parsedArgs.First(z => z.Item1 == "--eip").Item2;

            //run queue manager
            qm.Run();

            //run server
            Console.WriteLine($"Command reciever starting (port: {port})");
            Server server = new Server();
            server.Init(port);
            server.th.Join();
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
