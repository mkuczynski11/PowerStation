using Generator.Entities;

namespace Generator.Datastores;

public interface ISensorDatastore
{
    List<Sensor> CoreTempSensors { get; set; }
    List<Sensor> PowerGeneratedSensors { get; set; }
    List<Sensor> TurbinesRpmSensors { get; set; }
    List<Sensor> WaterUsageSensors { get; set; }
}