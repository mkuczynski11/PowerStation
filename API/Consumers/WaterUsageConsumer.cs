using System.Threading.Tasks;
using API.Common;
using API.Measurement.Entity;
using API.Measurement.Service;
using Common;
using MassTransit;

namespace API.Consumers
{
    public class WaterUsageConsumer: IConsumer<IWaterUsageMessage>
    {
        private readonly MeasurementService _service;

        public WaterUsageConsumer(MeasurementService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<IWaterUsageMessage> context)
        {
            var sensorID = context.Message.ID;
            var timestamp = context.Message.Timestamp;
            var value = context.Message.Value;
            var sensorType = SensorType.WATER_USAGE;

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