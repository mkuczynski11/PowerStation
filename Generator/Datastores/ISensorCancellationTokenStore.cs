using Generator.Tasks;

namespace Generator.Datastores;

public interface ISensorTaskStore
{
    List<CancellationTokenSource> CoreTempTokenSources { get; set; }
    List<CancellationTokenSource> PowerGeneratedTokenSources { get; set; }
    List<CancellationTokenSource> TurbinesRpmTokenSources { get; set; }
    List<CancellationTokenSource> WaterUsageTokenSources { get; set; }
}