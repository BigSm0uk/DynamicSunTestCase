using Core.Abstraction;
using Core.Domain;
using DataAccess.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class WeatherRepository(WeatherDbContext dataContext) : IRepository<WeatherEntity>
{
    private WeatherDbContext DataContext { get; set; } = dataContext;

    public async Task<IEnumerable<WeatherEntity?>> GetNFirstAsync(int take)
    {
        var builder = new WeatherQueryBuilder(DataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithPagination(1, take).Build().ToListAsync();
    }

    public async Task<IEnumerable<WeatherEntity?>> GetNFromPageAsync(int pageNumber, int pageSize)
    {
        var builder = new WeatherQueryBuilder(DataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithPagination(pageNumber, pageSize).Build().ToListAsync();
    }

    public async Task<IEnumerable<WeatherEntity?>> GetWithYearNavigationAsync(int pageNumber, int pageSize, int year)
    {
        var builder = new WeatherQueryBuilder(DataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithYearNavigation(year).WithPagination(pageNumber, pageSize).Build().ToListAsync();
    }

    public async Task<IEnumerable<WeatherEntity?>> GetNWithMonthNavigationAsync(int pageNumber, int pageSize, int month)
    {
        var builder = new WeatherQueryBuilder(DataContext.WeatherEntities.AsNoTracking());
        return await builder.WithDateOrderBy().WithMonthNavigation(month).WithPagination(pageNumber, pageSize).Build().ToListAsync();
    }

    public async Task CreateNewEntityAsync(WeatherEntity entity)
    {
        await DataContext.WeatherEntities.AddAsync(entity);
        await DataContext.SaveChangesAsync();
    }

    public async Task AddNewEntitiesAsync(IEnumerable<WeatherEntity> entities)
    {
        await DataContext.WeatherEntities.AddRangeAsync(entities);
        await DataContext.SaveChangesAsync();
    }

    public async Task DeleteAllAsync()
    {
        var entities = await DataContext.WeatherEntities.ToListAsync();

        DataContext.WeatherEntities.RemoveRange(entities);

        await DataContext.SaveChangesAsync();
    }
}