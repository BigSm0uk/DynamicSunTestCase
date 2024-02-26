using Core.Domain;

namespace Core.Abstraction;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T?>> GetMultipleWeatherEntitiesAsync(int take = default);
    public Task<IEnumerable<T?>> GetMultipleWeatherEntitiesByPageAsync(int pageNumber, int pageSize);

    public Task CreateNewEntityAsync(T entity);

    public void AddNewEntities(IEnumerable<T> entities);


    public Task<int> DeleteAllAsync();
}
