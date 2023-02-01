using System.Collections.Generic;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Interfaces;
using Autoglass.Domain.Models;

namespace Autoglass.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private const int maxSuppliersPageSize = 20;
        
        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        
        public async Task<IList<Supplier>> GetAllAsync(
            string? description,
            string? cnpj,
            int pageSize,
            int pageNumber)
        {
            if (pageSize > maxSuppliersPageSize)
            {
                pageSize = maxSuppliersPageSize;
            }
            
            return await _supplierRepository.GetAllAsync(
                description,
                cnpj,
                pageSize,
                pageNumber);
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            return await _supplierRepository.GetByIdAsync(id);
        }

        public async Task<Supplier> RegisterAsync(CreateSupplierViewModel model)
        {
            return await _supplierRepository.AddAsync(model);
        }

        public async Task<Supplier> UpdateAsync(UpdateSupplierViewModel model)
        {
            return await _supplierRepository.UpdateAsync(model);
        }
        
    }
}