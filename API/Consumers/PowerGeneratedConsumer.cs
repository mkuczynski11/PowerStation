using System.Threading.Tasks;
using API.Common;
using API.Measurement.Entity;
using API.Measurement.Service;
using Common;
using MassTransit;

namespace API.Consumers
{
    public class PowerGeneratedConsumer: IConsumer<MeasurementMessage>
    {
        private readonly MeasurementService _service;

        public PowerGeneratedConsumer(MeasurementService service)
        {
            _service = service;
        }
        
        public Task Consume(ConsumeContext<MeasurementMessage> context)
        {
            var sensorID = context.Message.ID;
            var timestamp = context.Message.Timestamp;
            var value = context.Message.Value;
            var sensorType = SensorType.POWER_GENERATED;

            var entity = new MeasurementEntity
            {
                SensorID = sensorID,
                SensorType = sensorType,
                Timestamp = timestamp,
                Value = value
            };
            
            _service.Create(entity);
            return Task.CompletedTask;
        }
    }
}