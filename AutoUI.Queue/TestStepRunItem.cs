namespace AutoUI.Queue
{
    public class TestStepRunItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestRunItem Parent { get; set; }
        public int ParentId { get; set; }
        public int TestStepIndex { get; set; }
        public RunStatus Status { get; set; }
        public int Duration { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
