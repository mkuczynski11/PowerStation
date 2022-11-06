namespace Common
{
    public interface ICoreTempMessage
    {
        int ID { get; set; }
        long Timestamp { get; set; }
        double Value { get; set; }
    }
}