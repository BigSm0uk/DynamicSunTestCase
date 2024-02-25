using Core.Domain;

namespace DataAccess.Builders;

public class WeatherQueryBuilder(IQueryable<WeatherEntity> queryable)
{
    public WeatherQueryBuilder WithDateOrderBy()
    {
        queryable = queryable.OrderBy(x => x.Date);
        return this;
    }

    public WeatherQueryBuilder WithPagination(int pageNumber, int pageSize)

    {
        queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return this;
    }

    public WeatherQueryBuilder WithYearNavigation(int year)
    {
        queryable = queryable.Where(x => x.Date.Year == year);
        return this;
    }

    public WeatherQueryBuilder WithMonthNavigation(int month)
    {
        queryable = queryable.Where(x => x.Date.Month == month);
        return this;
    }

    public IQueryable<WeatherEntity> Build()
    {
        return queryable;
    }
}