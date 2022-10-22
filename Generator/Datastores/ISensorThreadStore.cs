namespace Generator.Datastores;

public interface ISensorThreadStore
{
    List<Thread> CoreTempThreads { get; set; }
    List<Thread> PowerGeneratedSensors { get; set; }
    List<Thread> TurbinesRpmSensors { get; set; }
    List<Thread> WaterUsageSensors { get; set; }
}