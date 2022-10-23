using System;
using System.Collections.Generic;
using API.Measurement.Entity;
using MongoDB.Driver;

namespace API.Query
{
    public class MeasurementFilter
    {
        public int SensorID { get; private set; }
        public string SensorType { get; private set; }
        public long TimestampFrom { get; private set; }
        public long TimestampTo { get; private set; }
        
        public bool FilterBySensorID { get; private set; }
        public bool FilterBySensorType { get; private set; }
        public bool FilterByTimestampFrom { get; private set; }
        public bool FilterByTimestampTo { get; private set; }

        public bool ShouldFilter()
        {
            return FilterBySensorID ||
                   FilterBySensorType ||
                   FilterByTimestampFrom ||
                   FilterByTimestampTo;
        }

        public FilterDefinition<MeasurementEntity> ToFilterDefinition()
        {
            if (!ShouldFilter()) return null;
            var filters = new List<FilterDefinition<MeasurementEntity>>();

            if (FilterBySensorID)
            {
                filters.Add(Builders<MeasurementEntity>.Filter.Eq("sensor_id", SensorID));
            }
            if (FilterBySensorType)
            {
                filters.Add(Builders<MeasurementEntity>.Filter.Eq("sensor_type", SensorType));
            }
            if (FilterByTimestampFrom)
            {
                filters.Add(Builders<MeasurementEntity>.Filter.Gte("timestamp", TimestampFrom));
            }
            if (FilterByTimestampTo)
            {
                filters.Add(Builders<MeasurementEntity>.Filter.Lte("timestamp", TimestampTo));
            }
            
            return Builders<MeasurementEntity>.Filter.And(filters);
        }

        public MeasurementFilter WithSensorID(string id)
        {
            FilterBySensorID = Int32.TryParse(id, out var value);
            SensorID = value;
            return this;
        }
        
        public MeasurementFilter WithSensorType(string type)
        {
            FilterBySensorType = type != null;
            SensorType = type;
            return this;
        }
        
        public MeasurementFilter WithTimestampFrom(string from)
        {
            FilterByTimestampFrom = Int64.TryParse(from, out var value);
            TimestampFrom = value;
            return this;
        }
        
        public MeasurementFilter WithTimestampTo(string to)
        {
            FilterByTimestampTo = Int64.TryParse(to, out var value);
            TimestampTo = value;
            return this;
        }
    }
}