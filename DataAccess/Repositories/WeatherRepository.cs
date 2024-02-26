using Core.Abstraction;
using Core.Domain;
using DataAccess.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class WeatherRepository(WeatherDbContext dataContext) : IRepository<WeatherEntity>, IDisposable
{
    public async Task<IEnumerable<WeatherEntity?>> GetMultipleWeatherEntitiesAsync(int take)
    {
        var builder = new WeatherQueryBuilder(dataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithPagination(1, take).Build().ToListAsync();
    }

    public async Task<IEnumerable<WeatherEntity?>> GetMultipleWeatherEntitiesByPageAsync(int pageNumber, int pageSize)
    {
        var builder = new WeatherQueryBuilder(dataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithPagination(pageNumber, pageSize).Build().ToListAsync();
    }

    public async Task<IEnumerable<WeatherEntity?>> GetMultipleWeatherEntitiesByYearAsync(int pageNumber, int pageSize, int year)
    {
        var builder = new WeatherQueryBuilder(dataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithYearNavigation(year).WithPagination(pageNumber, pageSize).Build().ToListAsync();
    }

    public async Task<IEnumerable<WeatherEntity?>> GetMultipleWeatherEntitiesByMonthAsync(int pageNumber, int pageSize, int month)
    {
        var builder = new WeatherQueryBuilder(dataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithMonthNavigation(month).WithPagination(pageNumber, pageSize).Build().ToListAsync();
    }

    public async Task CreateNewEntityAsync(WeatherEntity entity)
    {
        dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
        await dataContext.WeatherEntities.AddAsync(entity);
        await dataContext.SaveChangesAsync();
        dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
    }
    

    public void AddNewEntities(IEnumerable<WeatherEntity> entities)
    {
        dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
        dataContext.WeatherEntities.AddRange(entities);
        dataContext.SaveChanges();
        dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
    }

    public async Task<int> DeleteAllAsync()
    {
        var count = dataContext.WeatherEntities.Count();
        dataContext.WeatherEntities.RemoveRange(dataContext.WeatherEntities);
        await dataContext.SaveChangesAsync();
        return count;
    }

    public void Dispose()
    {
        dataContext.Dispose();
        
    }
}