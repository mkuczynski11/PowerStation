using Generator.Datastores;
using Generator.Entities;
using Generator.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Generator.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GeneratorController : ControllerBase
{
    private readonly ILogger<GeneratorController> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    
    private readonly IGeneratorState _generatorState;
    private readonly ISensorDatastore _sensorDatastore;
    private readonly ISensorTaskStore _sensorTaskStore;

    public GeneratorController(ILogger<GeneratorController> logger, ILoggerFactory loggerFactory, IPublishEndpoint publishEndpoint, 
        IGeneratorState generatorState, ISensorDatastore sensorDatastore, ISensorTaskStore sensorTaskStore)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _publishEndpoint = publishEndpoint;
        
        _generatorState = generatorState;
        _sensorDatastore = sensorDatastore;
        _sensorTaskStore = sensorTaskStore;
    }
    
    [HttpGet]
    public IActionResult StartGeneration()
    {
        if (_generatorState.Generating == false)
        {
            //CoreTemp
            foreach (Sensor sensor in _sensorDatastore.CoreTempSensors)
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                SensorTask sensorTask = new SensorTask(sensor, _loggerFactory.CreateLogger<SensorTask>(), _publishEndpoint);
                _sensorTaskStore.CoreTempTokenSources.Add(cancellationTokenSource);
                var task = new Task(() => sensorTask.SensorSendingTask(token));
                task.Start();
            }
            _logger.LogInformation("CoreTemp tasks amount: " + _sensorTaskStore.CoreTempTokenSources.Count);
            
             //PowerGenerated
             foreach (Sensor sensor in _sensorDatastore.PowerGeneratedSensors)
             {
                 var cancellationTokenSource = new CancellationTokenSource();
                 var token = cancellationTokenSource.Token;
                 SensorTask sensorTask = new SensorTask(sensor, _loggerFactory.CreateLogger<SensorTask>(), _publishEndpoint);
                 _sensorTaskStore.PowerGeneratedTokenSources.Add(cancellationTokenSource);
                 var task = new Task(() => sensorTask.SensorSendingTask(token));
                 task.Start();
             }
             _logger.LogInformation("PowerGenerated tasks count: " + _sensorTaskStore.PowerGeneratedTokenSources.Count);
            
             //TurbineRPM
             foreach (Sensor sensor in _sensorDatastore.TurbinesRpmSensors)
             {
                 var cancellationTokenSource = new CancellationTokenSource();
                 var token = cancellationTokenSource.Token;
                 SensorTask sensorTask = new SensorTask(sensor, _loggerFactory.CreateLogger<SensorTask>(), _publishEndpoint);
                 _sensorTaskStore.TurbinesRpmTokenSources.Add(cancellationTokenSource);
                 var task = new Task(() => sensorTask.SensorSendingTask(token));
                 task.Start();
             }
             _logger.LogInformation("TurbineRPM tasks count: " + _sensorTaskStore.TurbinesRpmTokenSources.Count);
            
             //WaterUsage
             foreach (Sensor sensor in _sensorDatastore.WaterUsageSensors)
             {
                 var cancellationTokenSource = new CancellationTokenSource();
                 var token = cancellationTokenSource.Token;
                 SensorTask sensorTask = new SensorTask(sensor, _loggerFactory.CreateLogger<SensorTask>(), _publishEndpoint);
                 _sensorTaskStore.WaterUsageTokenSources.Add(cancellationTokenSource);
                 var task = new Task(() => sensorTask.SensorSendingTask(token));
                 task.Start();
             }
             _logger.LogInformation("WaterUsage tasks count: " + _sensorTaskStore.WaterUsageTokenSources.Count);
            
            _logger.LogInformation("Generator started");
            _generatorState.Generating = true;
        }
        else
        {
            _logger.LogInformation("Generator is already running");
        }

        return Ok();
    }
    
    [HttpGet]
    public IActionResult StopGeneration()
    {
        if (_generatorState.Generating)
        {
            foreach (var task in _sensorTaskStore.CoreTempTokenSources)
            {
                task.Cancel();
                task.Dispose();
            }
            _sensorTaskStore.CoreTempTokenSources.Clear();

            foreach (var task in _sensorTaskStore.PowerGeneratedTokenSources)
            {
                task.Cancel();
                task.Dispose();
            }
            _sensorTaskStore.PowerGeneratedTokenSources.Clear();
            
            foreach (var task in _sensorTaskStore.TurbinesRpmTokenSources)
            {
                task.Cancel();
                task.Dispose();
            }
            _sensorTaskStore.TurbinesRpmTokenSources.Clear();
            
            foreach (var task in _sensorTaskStore.WaterUsageTokenSources)
            {
                task.Cancel();
                task.Dispose();
            }
            _sensorTaskStore.WaterUsageTokenSources.Clear();

            _logger.LogInformation("Generator stopped");
            _generatorState.Generating = false;
        }
        else
        {
            _logger.LogInformation("Generator is already stopped");
        }
        return Ok();
    }
    
    [HttpGet]
    public List<List<Sensor>> GetSensorsInfo()
    {
        _logger.LogInformation("Getting sensor info");
        _logger.LogInformation("CoreTemp tasks: " + _sensorTaskStore.CoreTempTokenSources.Count);
        _logger.LogInformation("PowerGenerated tasks: " + _sensorTaskStore.PowerGeneratedTokenSources.Count);
        _logger.LogInformation("TurbineRPM tasks: " + _sensorTaskStore.TurbinesRpmTokenSources.Count);
        _logger.LogInformation("WaterUsage tasks: " + _sensorTaskStore.WaterUsageTokenSources.Count);
        return new List<List<Sensor>>
        {
            _sensorDatastore.CoreTempSensors,
            _sensorDatastore.PowerGeneratedSensors,
            _sensorDatastore.TurbinesRpmSensors,
            _sensorDatastore.WaterUsageSensors
        };
    }
}