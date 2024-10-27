namespace SDD.Services.ProductAPI.Contracts
{
    public interface IProductRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellation = default);
        Task<T> AddAsync(T entity, CancellationToken cancellation = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellation = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellation = default);
        Task<IEnumerable<T>> GetAllAsync(int skip, int take, CancellationToken cancellation = default);
    }
}
