using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class RemoteRunner
    {
        public static async Task<TestRunContext> RunRemotely(StreamWriter wr,
            StreamReader rdr,
            int testIdx,
            string testParams = null)
        {
            await wr.WriteLineAsync($"RUN_TEST={testIdx};{testParams ?? string.Empty}");
            await wr.FlushAsync();
            var res = await rdr.ReadLineAsync();
            
            var sub = res.Substring("RESULT=".Length);

            //var tt = Enum.Parse<TestStateEnum>(spl[0]);
            return new TestRunContext(XDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(sub))));
        }
    }
}
