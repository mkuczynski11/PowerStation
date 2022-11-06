using Generator.Tasks;

namespace Generator.Datastores;

public class SensorTaskStore : ISensorTaskStore
{
    public List<CancellationTokenSource> CoreTempTokenSources { get; set; }
    public List<CancellationTokenSource> PowerGeneratedTokenSources { get; set; }
    public List<CancellationTokenSource> TurbinesRpmTokenSources { get; set; }
    public List<CancellationTokenSource> WaterUsageTokenSources { get; set; }

    public SensorTaskStore()
    {
        CoreTempTokenSources = new List<CancellationTokenSource>();
        PowerGeneratedTokenSources = new List<CancellationTokenSource>();
        TurbinesRpmTokenSources = new List<CancellationTokenSource>();
        WaterUsageTokenSources = new List<CancellationTokenSource>();
    }
}