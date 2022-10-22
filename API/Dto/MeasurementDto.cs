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
        /// Measurement Timestamp
        /// </summary>
        /// <example>1256953732</example>
        public long Timestamp { get; set; }
        /// <summary>
        /// Measurement Value
        /// </summary>
        /// <example>103.23</example>
        public double Value { get; set; }
    }
}