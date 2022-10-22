using System;

namespace Common
{
    public interface MeasurementMessage
    {
        int ID { get; set; }
        long Timestamp { get; set; }
        double Value { get; set; }
    }
}