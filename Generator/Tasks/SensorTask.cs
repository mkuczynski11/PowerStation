using Generator.Entities;
using RabbitMQ.Client;

namespace Generator.Tasks;

public class SensorTask
{
    private Sensor _sensor;
    private float _currentVal;
    private ConnectionFactory _factory;
    private IConnection _conn;
    private IModel _channel;

    public volatile bool Cancel;
    
    public SensorTask(Sensor sensor)
    {
        _sensor = sensor;
        _currentVal = (sensor.MinValue + sensor.MaxValue) / 2;

        _factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
        _conn = _factory.CreateConnection();
        _channel = _conn.CreateModel();
        _channel.QueueDeclare(
            queue: "sensors",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

    }
}