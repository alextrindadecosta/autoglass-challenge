using System.Collections.Generic;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Models;

namespace Autoglass.Domain.Interfaces
{
    public interface IProductService
    {
        Task<IList<Product>> GetAllAsync(
            string? description, 
            int? supplier, 
            string? manufactureDate, 
            string? expirationDate,
            int pageSize,
            int pageNumber);
        Task<Product> GetByIdAsync(int id);
        Task<Product> RegisterAsync(CreateProductViewModel model);
        Task<Product> UpdateAsync(UpdateProductViewModel model);
        Task<Product> RemoveAsync(int id);
    }
}