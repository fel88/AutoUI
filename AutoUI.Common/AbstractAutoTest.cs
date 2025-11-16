namespace AutoUI.Common
{
    public abstract class AbstractAutoTest
    {
        public TestFailedBehaviour FailedAction { get; set; }
        public TestStateEnum State { get; set; }

        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

    }

}
