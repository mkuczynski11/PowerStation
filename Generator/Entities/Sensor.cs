namespace Generator.Entities;

public class Sensor
{
    public int SensorId { get; set; }
    public string SensorName { get; set; }
    public float MinValue { get; set; }
    public float MaxValue { get; set; }
    public float StepValue { get; set; }
    public float SendTimeSeconds { get; set; }

    public Sensor(int sensorId, string sensorName, float minValue, float maxValue, float stepValue, float sendTimeSeconds)
    {
        this.SensorId = sensorId;
        this.SensorName = sensorName;
        this.MinValue = minValue;
        this.MaxValue = maxValue;
        this.StepValue = stepValue;
        this.SendTimeSeconds = sendTimeSeconds;
    }
}