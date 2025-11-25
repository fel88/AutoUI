namespace AutoUI.Queue
{
    public class Run
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public RunStatus Status { get; set; }
        public List<TestRunItem> Tests { get; set; } = new List<TestRunItem>();
        public string Xml { get; set; }// run params
        public string ResultDescription { get; set; }
        public string ResultXml { get; set; }
    }
}
