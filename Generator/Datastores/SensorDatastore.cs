using Generator.Entities;

namespace Generator.Datastores;

public class SensorDatastore : ISensorDatastore
{
   public List<Sensor> CoreTempSensors { get; set; }

    public List<Sensor> PowerGeneratedSensors { get; set; }

    public List<Sensor> TurbinesRpmSensors { get; set; }

    public List<Sensor> WaterUsageSensors { get; set; }

    public SensorDatastore()
    {
        CoreTempSensors = new List<Sensor>
        {
            new Sensor(1, "CoreTemp", 750.0f, 950.0f, 2.0f, 1.0f),
            new Sensor(2, "CoreTemp", 750.0f, 950.0f, 2.0f, 1.0f),
            new Sensor(3, "CoreTemp", 775.0f, 975.0f, 2.0f, 1.1f),
            new Sensor(4, "CoreTemp", 800.0f, 1000.0f, 3.0f, 1.2f),
            new Sensor(5, "CoreTemp", 825.0f, 1025.0f, 3.0f, 1.3f),
            new Sensor(6, "CoreTemp", 850.0f, 1050.0f, 3.0f, 1.4f),
            new Sensor(7, "CoreTemp", 850.0f, 1050.0f, 3.0f, 1.4f),
            new Sensor(8, "CoreTemp", 850.0f, 1050.0f, 3.0f, 1.4f)
        };
        
        PowerGeneratedSensors = new List<Sensor>
        {
            new Sensor(9, "PowerGenerated", 500.0f, 550.0f, 1.0f, 2),
            new Sensor(10, "PowerGenerated", 350.0f, 375.0f, 1.0f, 2),
            new Sensor(11, "PowerGenerated", 450.0f, 500.0f, 1.0f, 2),
            new Sensor(12, "PowerGenerated", 500.0f, 550.0f, 0.7f, 2),
            new Sensor(13, "PowerGenerated", 350.0f, 375.0f, 0.7f, 2),
            new Sensor(14, "PowerGenerated", 450.0f, 500.0f, 0.7f, 2),
            new Sensor(15, "PowerGenerated", 450.0f, 500.0f, 0.7f, 2),
            new Sensor(16, "PowerGenerated", 450.0f, 500.0f, 0.7f, 2)
        }; 
        
        TurbinesRpmSensors = new List<Sensor>
        {
            new Sensor(17, "TurbineRPM", 1800.0f, 3600.0f, 100.0f, 5),
            new Sensor(18, "TurbineRPM", 1800.0f, 3600.0f, 100.0f, 5),
            new Sensor(19, "TurbineRPM", 1800.0f, 3600.0f, 100.0f, 5),
            new Sensor(20, "TurbineRPM", 4000.0f, 5000.0f, 75.0f, 5),
            new Sensor(21, "TurbineRPM", 4000.0f, 5000.0f, 75.0f, 5),
            new Sensor(22, "TurbineRPM", 4000.0f, 5000.0f, 75.0f, 5),
            new Sensor(23, "TurbineRPM", 4000.0f, 5000.0f, 75.0f, 5),
            new Sensor(24, "TurbineRPM", 4000.0f, 5000.0f, 75.0f, 5)
        };
        
        WaterUsageSensors = new List<Sensor>
        {
            new Sensor(25, "WaterUsage", 151.4f, 272.5f, 3.0f, 2.5f),
            new Sensor(26, "WaterUsage", 152.4f, 272.5f, 3.5f, 2.5f),
            new Sensor(27, "WaterUsage", 153.4f, 272.5f, 3.75f, 2.5f),
            new Sensor(28, "WaterUsage", 154.4f, 272.5f, 4.0f, 3.0f),
            new Sensor(29, "WaterUsage", 155.4f, 272.5f, 5.0f, 3.0f),
            new Sensor(30, "WaterUsage", 156.4f, 272.5f, 6.0f, 3.0f),
            new Sensor(31, "WaterUsage", 156.4f, 272.5f, 6.0f, 3.0f),
            new Sensor(32, "WaterUsage", 156.4f, 272.5f, 6.0f, 3.0f)
        };
    }
}