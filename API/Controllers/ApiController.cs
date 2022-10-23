using System.Collections.Generic;
using System.Globalization;
using System.IO;
using API.Measurement.Dto;
using API.Measurement.Entity;
using API.Measurement.Service;
using API.Query;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly MeasurementService _service;

        public ApiController(ILogger<ApiController> logger, MeasurementService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <returns>Some response</returns>
        /// <response code="418">Who knows</response>
        [HttpPost("test")]
        public IActionResult Post(MeasurementDto dto)
        {
            var entity = new MeasurementEntity
            {
                SensorID = dto.SensorID,
                Timestamp = dto.Timestamp,
                SensorType = dto.SensorType,
                Value = dto.Value
            };

            _service.Create(entity);

            return StatusCode(418, _service.FindAll());
        }

        /// <summary>
        /// Gets Measurements
        /// </summary>
        /// <returns>List of Measurements</returns>
        /// <response code="200">Request Successful</response>
        [HttpGet]
        public IEnumerable<MeasurementDto> Get(
            [FromQuery] string sensorID,
            [FromQuery] string sensorType,
            [FromQuery] string timestampFrom,
            [FromQuery] string timestampTo,
            [FromQuery] string sortBy,
            [FromQuery] string sort)
        {
            return new List<MeasurementEntity>(_service.FindAllWithFilteredAndSorted(
                new MeasurementFilter()
                    .WithSensorID(sensorID)
                    .WithSensorType(sensorType)
                    .WithTimestampFrom(timestampFrom)
                    .WithTimestampTo(timestampTo), 
                new MeasurementSort()
                    .WithSortBy(sortBy, sort)))
                .ConvertAll(MeasurementDto.EntityToDtoConverter);
        }

        /// <summary>
        /// Gets Measurements as csv
        /// </summary>
        /// <returns>Csv file with Measurements</returns>
        /// <response code="200">Request Successful</response>
        [HttpGet("csv")]
        public FileResult Download(
            [FromQuery] string sensorID,
            [FromQuery] string sensorType,
            [FromQuery] string timestampFrom,
            [FromQuery] string timestampTo,
            [FromQuery] string sortBy,
            [FromQuery] string sort)
        {
            var dtos = new List<MeasurementEntity>(_service.FindAllWithFilteredAndSorted(
                    new MeasurementFilter()
                        .WithSensorID(sensorID)
                        .WithSensorType(sensorType)
                        .WithTimestampFrom(timestampFrom)
                        .WithTimestampTo(timestampTo), 
                    new MeasurementSort()
                        .WithSortBy(sortBy, sort)))
                .ConvertAll(MeasurementDto.EntityToDtoConverter);
            
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(dtos);
                csv.Flush();
                ms.Position = 0;
                return File(ms.ToArray(), "text/csv", "data.csv");
            }
        }
    }
}