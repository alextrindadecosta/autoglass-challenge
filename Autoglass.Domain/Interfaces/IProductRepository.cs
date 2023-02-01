using System.Collections.Generic;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Models;

namespace Autoglass.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAllAsync(
            string? description, 
            int? supplierId, 
            string? manufactureDate, 
            string? expirationDate,
            int pageSize,
            int pageNumber);
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(CreateProductViewModel model);
        Task<Product> UpdateAsync(UpdateProductViewModel model);
        Task<Product> RemoveAsync(int id);
    }
}