namespace AutoUI.Common
{
    public static class DefaultScripts
    {
        public static string DefaultTestScript = @"using System;
using System.IO;
using AutoUI.Common;
using System.Linq;
using AutoUI.Common;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OBSWebsocketDotNet;

class Program : IRun
{
    public void Run(TestRunContext ctx)
    {
    }
}"; 
        
        public static string DefaultTestSetScript = @"using System;
using System.IO;
using AutoUI.Common;
using System.Linq;
using AutoUI.Common;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OBSWebsocketDotNet;

class Program : ISetRun
{
    public void Run(SetRunContext set)
    {
    }
}";

    }
}
