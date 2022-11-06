using Common;

namespace Generator.Entities;

public class MeasuredMessage : ICoreTempMessage, IPowerGeneratedMessage, ITurbineRpmMessage, IWaterUsageMessage
{
    public int ID { get; set; }
    public long Timestamp { get; set; }
    public double Value { get; set; }

    public MeasuredMessage(int id, long timestamp, double value)
    {
        ID = id;
        Timestamp = timestamp;
        Value = value;
    }
}