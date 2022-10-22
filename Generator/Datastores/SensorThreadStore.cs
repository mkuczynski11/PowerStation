namespace Generator.Datastores;

public class SensorThreadStore : ISensorThreadStore
{
    public List<Thread> CoreTempThreads { get; set; }
    public List<Thread> PowerGeneratedSensors { get; set; }
    public List<Thread> TurbinesRpmSensors { get; set; }
    public List<Thread> WaterUsageSensors { get; set; }
}