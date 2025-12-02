namespace AutoUI.Queue
{
    public class TestRunItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Run Parent { get; set; }
        public int ParentId { get; set; }
        public int TestIndex { get; set; }
        public RunStatus Status { get; set; }
        public int Duration { get; set; }
        public DateTime Timestamp { get; set; }
        public string XmlOutput { get; set; }

        public List<TestStepRunItem> Steps { get; set; } = new List<TestStepRunItem>();
    }
}
