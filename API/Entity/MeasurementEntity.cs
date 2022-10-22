using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Entity
{
    public class MeasurementEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        
        [BsonElement]
        public int SensorID { get; set; }
        
        [BsonElement]
        public long Timestamp { get; set; }
        
        [BsonElement]
        public double Value { get; set; }

        public MeasurementEntity()
        {
        }

        public MeasurementEntity(ObjectId id, int sensorId, long timestamp, double value)
        {
            _id = id;
            SensorID = sensorId;
            Timestamp = timestamp;
            Value = value;
        }
    }
}