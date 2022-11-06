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
    private bool _valueAddition;

    public SensorTask(Sensor sensor, ILogger<SensorTask> logger, IPublishEndpoint publishEndpoint)
    {
        _sensor = sensor;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _currentVal = (sensor.MinValue + sensor.MaxValue) / 2;
        _valueAddition = true;
        _logger.LogInformation("I am a task with sensor: " + sensor);
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
        if (_valueAddition)
        {
            _currentVal += _sensor.StepValue;
            if (_currentVal >= _sensor.MaxValue)
                _valueAddition = false;
        }
        else
        {
            _currentVal -= _sensor.StepValue;
            if (_currentVal <= _sensor.MinValue)
                _valueAddition = true;
        }
    }
}