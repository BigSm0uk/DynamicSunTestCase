using Core.Domain;

namespace Core.Abstraction;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T?>> GetNFirstAsync(int take = default);
    public Task<IEnumerable<T?>> GetNFromPageAsync(int pageNumber, int pageSize);

    public Task CreateNewEntityAsync(T entity);

    public Task AddNewEntitiesAsync(IEnumerable<T> entities);


    public Task DeleteAllAsync();
}
