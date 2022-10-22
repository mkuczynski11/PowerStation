using System.Threading.Tasks;
using API.Configuration;
using API.Entity;
using Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace API.Consumers
{
    public class CoreTemperatureConsumer : IConsumer<MeasurementMessage>
    {
        private readonly MongoClient _mongoClient;
        private readonly MongoDbConfiguration _mongoConf;

        public CoreTemperatureConsumer(MongoClient mongoClient, IConfiguration configuration)
        {
            _mongoClient = mongoClient;
            _mongoConf = configuration.GetSection("MongoDb").Get<MongoDbConfiguration>();
        }

        public Task Consume(ConsumeContext<MeasurementMessage> context)
        {
            var sensorID = context.Message.ID;
            var timestamp = context.Message.Timestamp;
            var value = context.Message.Value;
            var sensorType = SensorType.CORE_TEMPERATURE;

            var entity = new MeasurementEntity
            {
                SensorID = sensorID,
                SensorType = sensorType,
                Timestamp = timestamp,
                Value = value
            };
            
            var collection = _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName);
            
            collection.InsertOne(entity);
            return Task.CompletedTask;
        }
    }
}