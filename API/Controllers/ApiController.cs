using System;
using System.Collections.Generic;
using System.Linq;
using API.Configuration;
using API.Dto;
using API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace API.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly MongoClient _mongoClient;
        private readonly MongoDbConfiguration _mongoConf;

        public ApiController(ILogger<ApiController> logger, MongoClient mongoClient, IConfiguration configuration)
        {
            _logger = logger;
            _mongoClient = mongoClient;
            _mongoConf = configuration.GetSection("MongoDb").Get<MongoDbConfiguration>();
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <returns>Some response</returns>
        /// <response code="418">Who knows</response>
        [HttpGet("test")]
        public IActionResult Get()
        {
            var entity = new MeasurementEntity
            {
                SensorID = 1,
                Timestamp = 1,
                Value = 1.2
            };
            
            var collection = _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName);
            
            collection.InsertOne(entity);
            // var dto = MeasurementDto.EntityToDtoMapper(entity);

            return StatusCode(418, collection.Find(_ => true).ToList());
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