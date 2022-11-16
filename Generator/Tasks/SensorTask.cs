using System.Text;
using Common;
using Generator.Entities;
using MassTransit;
using RabbitMQ.Client;

namespace Generator.Tasks;

public class SensorTask
{
    private readonly Sensor _sensor;
    private readonly ILogger<SensorTask> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private float _currentVal;
    private Random _random;

    public SensorTask(Sensor sensor, ILogger<SensorTask> logger, IPublishEndpoint publishEndpoint)
    {
        _sensor = sensor;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _currentVal = (sensor.MinValue + sensor.MaxValue) / 2;
        _logger.LogInformation("I am a task with sensor: " + sensor);
        _random = new Random();
    }

    public void SensorSendingTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var message = new MeasuredMessage(_sensor.SensorId,
                DateTimeOffset.Now.ToUnixTimeMilliseconds(), _currentVal);
            switch (_sensor.SensorName)
            {
                case SensorNames.Temp:
                    _publishEndpoint.Publish<ICoreTempMessage>(message);
                    break;
                case SensorNames.Power:
                    _publishEndpoint.Publish<IPowerGeneratedMessage>(message);
                    break;
                case SensorNames.Turbine:
                    _publishEndpoint.Publish<ITurbineRpmMessage>(message);
                    break;
                case SensorNames.Water:
                    _publishEndpoint.Publish<IWaterUsageMessage>(message);
                    break;
            }
            _logger.LogInformation("Sent message from: " + _sensor.SensorName);
            UpdateCurrentVal();
            Thread.Sleep((int)_sensor.SendTimeSeconds * 1500);
        }
    }

    private void UpdateCurrentVal()
    {
        float val = _currentVal;
        val = _random.Next() % 2 == 0 ? val + _sensor.StepValue : val - _sensor.StepValue;
        if (val <= _sensor.MaxValue && val >= _sensor.MinValue)
        {
            _currentVal = val;
        }
    }
}