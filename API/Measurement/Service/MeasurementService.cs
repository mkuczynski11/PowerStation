using System.Collections.Generic;
using API.Measurement.Entity;
using API.Measurement.Repository;
using API.Query;
using MongoDB.Driver;

namespace API.Measurement.Service
{
    public class MeasurementService
    {
        private readonly MeasurementRepository _repository;

        public MeasurementService(MeasurementRepository repository)
        {
            _repository = repository;
        }

        public void Create(MeasurementEntity entity)
        {
            _repository.Create(entity);
        }

        public IEnumerable<MeasurementEntity> FindAll()
        {
            return _repository.FindAll();
        }
        
        public IEnumerable<MeasurementEntity> FindAllWithFilteredAndSorted(MeasurementFilter filter, MeasurementSort sort)
        {
            if (filter.ShouldFilter() && !sort.ShouldSort())
            {
                return _repository.FindAllFiltered(filter.ToFilterDefinition());
            }
            if (!filter.ShouldFilter() && sort.ShouldSort())
            {
                return _repository.FindAllSorted(sort.ToSortDefinition());
            }
            if (filter.ShouldFilter() && sort.ShouldSort())
            {
                return _repository.FindAllFilteredAndSorted(filter.ToFilterDefinition(), sort.ToSortDefinition());
            }
            return _repository.FindAll();
        }
    }
}