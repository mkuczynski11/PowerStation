using API.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Measurement.Entity
{
    public class MeasurementEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        
        [BsonElement("sensor_id")]
        public int SensorID { get; set; }
        
        [BsonElement("sensor_type")]
        public string SensorType { get; set; }
        
        [BsonElement("timestamp")]
        public long Timestamp { get; set; }
        
        [BsonElement("value")]
        public double Value { get; set; }

        public MeasurementEntity()
        {
        }

        public override string ToString()
        {
            return $"Measurement=[SensorID:{SensorID.ToString()}," +
                   $"SensorType:{SensorType}," +
                   $"Timestamp={Timestamp.ToString()}," +
                   $"Value={Value.ToString()}]";
        }
    }
}