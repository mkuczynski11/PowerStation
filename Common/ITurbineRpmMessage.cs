namespace Common
{
    public interface ITurbineRpmMessage
    {
        public int ID { get; set; }
        public long Timestamp { get; set; }
        public double Value { get; set; }
    }
}