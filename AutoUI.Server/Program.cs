using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoUI.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8888;
            LoadConfig();
            var server = new Server();
            server.Init(port);
            server.th.Join();
        }

        private static void LoadConfig()
        {
            if (!File.Exists("config.xml"))
                return;

            var doc = XDocument.Load("config.xml");


        }
    }
}
