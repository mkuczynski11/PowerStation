using System.Collections.Generic;
using API.Configuration;
using API.Measurement.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace API.Measurement.Repository
{
    public class MeasurementRepository
    {
        private readonly ILogger<MeasurementRepository> _logger;
        private readonly MongoClient _mongoClient;
        private readonly MongoDbConfiguration _mongoConf;

        public MeasurementRepository(ILogger<MeasurementRepository> logger, MongoClient mongoClient, IConfiguration configuration)
        {
            _logger = logger;
            _mongoClient = mongoClient;
            _mongoConf = configuration.GetSection("MongoDb").Get<MongoDbConfiguration>();
        }

        public void Create(MeasurementEntity entity)
        {
            _logger.Log(LogLevel.Information, "Storing entity {S} in db", entity.ToString());
            _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName)
                .InsertOne(entity);
        }

        public IEnumerable<MeasurementEntity> FindAll()
        {
            _logger.Log(LogLevel.Information, "Return all entities from db");
            return _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName)
                .Find(_ => true).ToList();
        }

        public IEnumerable<MeasurementEntity> FindAllFiltered(FilterDefinition<MeasurementEntity> filter)
        {
            _logger.Log(LogLevel.Information, "Return filtered entities from db");
            return _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName)
                .Find(filter).ToList();
        }

        public IEnumerable<MeasurementEntity> FindAllSorted(SortDefinition<MeasurementEntity> sort)
        {
            _logger.Log(LogLevel.Information, "Return all entities sorted from db");
            return _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName)
                .Find(_ => true).Sort(sort).ToList();
        }

        public IEnumerable<MeasurementEntity> FindAllFilteredAndSorted(FilterDefinition<MeasurementEntity> filter,
            SortDefinition<MeasurementEntity> sort)
        {
            _logger.Log(LogLevel.Information, "Return filtered entities sorted from db");
            return _mongoClient.GetDatabase(_mongoConf.DatabaseName)
                .GetCollection<MeasurementEntity>(_mongoConf.CollectionName)
                .Find(filter).Sort(sort).ToList();
        }
    }
}