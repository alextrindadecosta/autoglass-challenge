using System.Collections.Generic;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Interfaces;
using Autoglass.Domain.Models;

namespace Autoglass.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private const int maxProductsPageSize = 20;
        
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<IList<Product>> GetAllAsync(
            string? description, 
            int? supplier, 
            string? manufactureDate, 
            string? expirationDate,
            int pageSize,
            int pageNumber)
        {
            if (pageSize > maxProductsPageSize)
            {
                pageSize = maxProductsPageSize;
            }
            
            return await _productRepository.GetAllAsync(
                description, 
                supplier, 
                manufactureDate, 
                expirationDate,
                pageSize,
                pageNumber);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> RegisterAsync(CreateProductViewModel model)
        {
            return await _productRepository.AddAsync(model);
        }

        public async Task<Product> UpdateAsync(UpdateProductViewModel model)
        {
            return await _productRepository.UpdateAsync(model);
        }

        public async Task<Product> RemoveAsync(int id)
        {
            return await _productRepository.RemoveAsync(id);
        }
    }
}
