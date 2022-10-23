using System.Collections.Generic;
using API.Measurement.Entity;
using MongoDB.Driver;

namespace API.Query
{
    public class MeasurementSort
    {
        private readonly HashSet<string> _permittedSortBy = new()
        {
            "sensor_id",
            "sensor_type",
            "timestamp",
            "value"
        };
        private bool _shouldSort;
        
        public string SortBy { get; private set; }
        public bool SortAsc { get; private set; }

        public bool ShouldSort()
        {
            return _shouldSort;
        }

        public SortDefinition<MeasurementEntity> ToSortDefinition()
        {
            if (!_shouldSort) return null;
            return SortAsc
                ? Builders<MeasurementEntity>.Sort.Ascending(SortBy)
                : Builders<MeasurementEntity>.Sort.Descending(SortBy);
        }

        public MeasurementSort WithSortBy(string sortBy, string sort)
        {
            _shouldSort = _permittedSortBy.Contains(sortBy);
            SortBy = sortBy;
            SortAsc = "asc".Equals(sort);
            return this;
        }
    }
}