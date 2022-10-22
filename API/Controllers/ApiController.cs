using System;
using System.Collections.Generic;
using System.Linq;
using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public MeasurementDto Get()
        {
            MeasurementDto mdto = new MeasurementDto
            {
                SensorID = 1,
                Timestamp = 1,
                Value = 1.2
            };
            return mdto;
        }

        /// <summary>
        /// Gets Measurements
        /// </summary>
        /// <returns>List of Measurements</returns>
        /// <response code="200">Request Successful</response>
        [HttpGet]
        public IEnumerable<MeasurementDto> Get(
            [FromQuery]string sensorID,
            [FromQuery]string sensorType,
            [FromQuery]string timestampFrom,
            [FromQuery]string timestampTo,
            [FromQuery]string sortBy,
            [FromQuery]string sort)
        {
            if (sensorID.Equals(String.Empty))
            {
                //No filter
            }
            return Enumerable.Empty<MeasurementDto>();
        }

        /// <summary>
        /// Gets Measurements as csv
        /// </summary>
        /// <returns>Csv file with Measurements</returns>
        /// <response code="200">Request Successful</response>
        [HttpGet("csv")]
        public FileResult Download(
            [FromQuery]string sensorID,
            [FromQuery]string sensorType,
            [FromQuery]string timestampFrom,
            [FromQuery]string timestampTo,
            [FromQuery]string sortBy,
            [FromQuery]string sort)
        {
            string fileName = "data.csv";
            byte[] fileBytes = { };
            return File(fileBytes, "text/csv", fileName);
        }
    }
}