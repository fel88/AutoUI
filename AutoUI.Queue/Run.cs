namespace AutoUI.Queue
{
    public class Run
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public RunStatus Status { get; set; }
        public List<TestRunItem> Tests { get; set; }
        public string Xml { get; set; }// run params
    }
}
