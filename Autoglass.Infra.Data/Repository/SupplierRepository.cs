using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Interfaces;
using Autoglass.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Autoglass.Infra.Data.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
         private readonly AppDbContext _context;
        
        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Supplier> GetByIdAsync(int id)
        {
            var supplier = await _context
                .Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return supplier;
        }

        public async Task<IList<Supplier>> GetAllAsync(
            string? description, 
            string? cnpj,
            int pageSize,
            int pageNumber)
        {
            var suppliers = _context.Suppliers as IQueryable<Supplier>;

            if (!string.IsNullOrWhiteSpace(description))
            {
                description = description.Trim();
                suppliers = suppliers.Where(p => p.Description.Contains(description));
            }
            
            if (!string.IsNullOrWhiteSpace(cnpj))
            {
                cnpj = cnpj.Trim();
                suppliers = suppliers.Where(p => p.CNPJ == cnpj);
            }
            
            return await suppliers.OrderBy(p => p.Description)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Supplier> AddAsync(CreateSupplierViewModel model)
        {
            var supplier = new Supplier(
                model.Description,
                model.CNPJ
            );
            
            try
            {
                await _context.Suppliers.AddAsync(supplier);
                await _context.SaveChangesAsync();
                return supplier;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao salvar o fornecedor.");
            }
        }

        public async Task<Supplier> UpdateAsync(UpdateSupplierViewModel model)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == model.Id);
            
            if (supplier == null)
            {
                throw new Exception("Não foi encontrado um fornecedor com o id fornecido.");
            }
            
            try
            {
                supplier.Description = model.Description;
                supplier.CNPJ = model.CNPJ;

                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();
                
                return supplier;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao atualizar o fornecedor.");
            }
        }
        
    }
}