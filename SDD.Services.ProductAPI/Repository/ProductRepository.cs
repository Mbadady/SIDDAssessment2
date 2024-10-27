using Microsoft.EntityFrameworkCore;
using SDD.Services.ProductAPI.Contracts;
using SDD.Services.ProductAPI.Data;

namespace SDD.Services.ProductAPI.Repository
{
    public class ProductRepository<T> : IProductRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellation = default)
        {
            await _dbSet.AddAsync(entity, cancellation);
            await _dbContext.SaveChangesAsync(cancellation);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellation = default)
        {
            var entity = await GetByIdAsync(id, cancellation);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellation);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync(int skip, int take,CancellationToken cancellation = default)
        {
            return await _dbSet.AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellation);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "ProductId") == id, cancellation);
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellation = default)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync(cancellation);
            return entity;
        }
    }
}
