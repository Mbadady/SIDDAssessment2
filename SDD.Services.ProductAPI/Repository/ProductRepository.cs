using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SDD.Services.ProductAPI.Contracts;
using SDD.Services.ProductAPI.Data;
using SDD.Services.ProductAPI.Models;
using SDD.Services.ProductAPI.Models.Dto;

namespace SDD.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddAsync(Product product, CancellationToken cancellation = default)
        {
            try
            {
                await _dbContext.Products.AddAsync(product, cancellation);
                await _dbContext.SaveChangesAsync(cancellation);
                var productDto = _mapper.Map<ProductDto>(product);
                return new ResponseDto { IsSuccess = true, Message = "Product created Successfully", Result = productDto };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = $"An error occurred while creating the product: {ex.Message}", Result = null };
            }
        }

        public async Task<ResponseDto> DeleteAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var entity = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id, cancellation);
                if (entity == null)
                {
                    return new ResponseDto { IsSuccess = true, Message = "No Product found for this id" };
                }

                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellation);
                return new ResponseDto { IsSuccess = true, Message = "Product deleted successfully" };
            }
            catch(Exception ex)
            {
                return new ResponseDto { IsSuccess = true, Message = $"An error occurred while deleting the product: {ex.Message}" };

            }

        }

        public async Task<ResponseDto> GetAllAsync(int skip, int take,CancellationToken cancellation = default)
        {
            try
            {
                var query = _dbContext.Products.AsNoTracking();
                var count = await query.CountAsync(cancellation);
                if (count == 0)
                {
                    return new ResponseDto { IsSuccess = true, Message = "No product found", Result = new List<Product>() };
                }
                var products = await query
                                                       .Skip(skip)
                                                       .Take(take)
                                                       .ToListAsync(cancellation);
                var productsDto = _mapper.Map<List<ProductDto>>(products);

                return new ResponseDto { IsSuccess = true, Message = $"{count} Products Fetched Successfully", Result = new { count, productsDto } };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = $"An error occurred while getting the products: {ex.Message}" };
            }
           
        }

        public async Task<ResponseDto> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == id, cancellation);
                if (product == null)
                {
                    return new ResponseDto { IsSuccess = true, Message = "No Product found for this id" };
                }
                var productDto = _mapper.Map<ProductDto>(product);

                return new ResponseDto { IsSuccess = true, Message = "Product found successfully", Result = productDto };
            }
            catch(Exception ex)
            {
                return new ResponseDto { IsSuccess = true, Message = $"An error occurred while getting the product: {ex.Message}", Result = null };

            }

        }

        public async Task<ResponseDto> UpdateAsync(Product product, CancellationToken cancellation = default)
        {
            try
            {
                // Check if the product exists in the database
                var existingProduct = await _dbContext.Products.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductId == product.ProductId, cancellation);

                if (existingProduct == null)
                {
                    return new ResponseDto { IsSuccess = false, Message = "No Product found for this id" };
                }

                // Update the product directly, since the 'product' object contains updated values
                _dbContext.Products.Update(product);
                // Save changes to the database
                await _dbContext.SaveChangesAsync(cancellation);
                var productDto = _mapper.Map<ProductDto>(product);

                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Product updated successfully",
                    Result = productDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false, // Marking it as failure for proper error indication
                    Message = $"An error occurred while updating the product: {ex.Message}",
                    Result = null
                };
            }
        }

    }
}
