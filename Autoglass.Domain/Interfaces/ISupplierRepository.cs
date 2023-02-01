using System.Collections.Generic;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Models;

namespace Autoglass.Domain.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IList<Supplier>> GetAllAsync(
            string? description,
            string? cnpj,
            int pageSize,
            int pageNumber);
        Task<Supplier> GetByIdAsync(int id);
        Task<Supplier> AddAsync(CreateSupplierViewModel model);
        Task<Supplier> UpdateAsync(UpdateSupplierViewModel model);
    }
}