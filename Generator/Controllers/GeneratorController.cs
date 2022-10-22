using Generator.Datastores;
using Generator.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Generator.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GeneratorController : ControllerBase
{
    private readonly ILogger<GeneratorController> _logger;
    private readonly IGeneratorState _generatorState;
    private readonly ISensorDatastore _sensorDatastore;
    
    public GeneratorController(ILogger<GeneratorController> logger, IGeneratorState generatorState, ISensorDatastore sensorDatastore)
    {
        _logger = logger;
        _generatorState = generatorState;
        _sensorDatastore = sensorDatastore;
    }
    
    [HttpGet]
    public IActionResult StartGeneration()
    {
        if (_generatorState.Generating == false)
        {
            foreach (Sensor sensor in _sensorDatastore.CoreTempSensors)
            {
                //Run threads to send data
            }
            
            foreach (Sensor sensor in _sensorDatastore.PowerGeneratedSensors)
            {
                //Run threads to send data
            }
            
            foreach (Sensor sensor in _sensorDatastore.TurbinesRpmSensors)
            {
                //Run threads to send data
            }
            
            foreach (Sensor sensor in _sensorDatastore.WaterUsageSensors)
            {
                //Run threads to send data
            }
            
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
        if (_generatorState.Generating == true)
        {
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
        return new List<List<Sensor>>
        {
            _sensorDatastore.CoreTempSensors,
            _sensorDatastore.PowerGeneratedSensors,
            _sensorDatastore.TurbinesRpmSensors,
            _sensorDatastore.WaterUsageSensors
        };
    }
}