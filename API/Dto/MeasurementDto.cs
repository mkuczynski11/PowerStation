using System;
using API.Entity;

namespace API.Dto
{
    public class MeasurementDto
    {
        /// <summary>
        /// Sensor ID
        /// </summary>
        /// <example>1</example>
        public int SensorID { get; set; }
        /// <summary>
        /// Sensor Type
        /// </summary>
        /// <example>coreTemperature</example>
        public string SensorType { get; set; }
        /// <summary>
        /// Measurement Timestamp
        /// </summary>
        /// <example>1256953732</example>
        public long Timestamp { get; set; }
        /// <summary>
        /// Measurement Value
        /// </summary>
        /// <example>103.23</example>
        public double Value { get; set; }

        public static Func<MeasurementEntity, MeasurementDto> EntityToDtoMapper = (entity) => new MeasurementDto()
        {
            SensorID = entity.SensorID,
            SensorType = entity.SensorType,
            Timestamp = entity.Timestamp,
            Value = entity.Value
        };
    }
}