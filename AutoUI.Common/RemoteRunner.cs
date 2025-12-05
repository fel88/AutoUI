using System.Text;
using System.Xml.Linq;

namespace AutoUI.Common
{
    public class RemoteRunner
    {
        public static async Task<TestRunContext> RunRemotely(StreamWriter wr,
            StreamReader rdr,
            int testIdx)
        {
            await wr.WriteLineAsync($"RUN_TEST={testIdx}");
            await wr.FlushAsync();
            var res = await rdr.ReadLineAsync();

            var sub = res.Substring("RESULT=".Length);

            //var tt = Enum.Parse<TestStateEnum>(spl[0]);
            return new TestRunContext(XDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(sub))));
        }
    }
}
