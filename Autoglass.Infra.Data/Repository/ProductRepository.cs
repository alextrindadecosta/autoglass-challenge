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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context
                .Products
                .Where(p => p.IsActive == true)
                .Include(p => p.Supplier)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return product;
        }

        public async Task<IList<Product>> GetAllAsync(
            string? description, 
            int? supplierId, 
            string? manufactureDate, 
            string? expirationDate,
            int pageSize,
            int pageNumber)
        {
            var products = _context.Products as IQueryable<Product>;

            if (!string.IsNullOrWhiteSpace(description))
            {
                description = description.Trim();
                products = products.Where(p => p.Description.Contains(description));
            }
            
            if (supplierId is not (<= 0 or null))
            {
                products = products.Where(p => p.SupplierId == supplierId);
            }
            
            if (!string.IsNullOrWhiteSpace(manufactureDate))
            {
                manufactureDate = manufactureDate.Trim();
                var searchDay = manufactureDate.Split("-")[0];
                var searchMonth = manufactureDate.Split("-")[1];
                var searchYear = manufactureDate.Split("-")[2];
                
                products = products.Where(p => (p.ManufactureDate.Day == int.Parse(searchDay)
                                                && p.ManufactureDate.Month == int.Parse(searchMonth)
                                                && p.ManufactureDate.Year == int.Parse(searchYear)));
            }
            
            if (!string.IsNullOrWhiteSpace(expirationDate))
            {
                expirationDate = expirationDate.Trim();
                var searchDay = expirationDate.Split("-")[0];
                var searchMonth = expirationDate.Split("-")[1];
                var searchYear = expirationDate.Split("-")[2];
                
                products = products.Where(p => (p.ExpirationDate.Day == int.Parse(searchDay)
                                                && p.ExpirationDate.Month == int.Parse(searchMonth)
                                                && p.ExpirationDate.Year == int.Parse(searchYear)));
            }

            return await products.OrderBy(p => p.Description)
                .Where(p => p.IsActive == true)
                .Include(p => p.Supplier)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product> AddAsync(CreateProductViewModel model)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == model.SupplierId);

            if (supplier == null)
            {
                throw new Exception("Não foi encontrado um fornecedor com o id fornecido.");
            }
            
            var product = new Product(
                model.Description,
                true,
                model.ManufactureDate,
                model.ExpirationDate,
                supplier.Id
            );

            product.Supplier = supplier;
            
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao salvar o produto.");
            }
        }

        public async Task<Product> UpdateAsync(UpdateProductViewModel model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (product == null)
            {
                throw new Exception("Produto não encontrado para realizar update.");
            }
            
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == model.SupplierId);
            
            if (supplier == null)
            {
                throw new Exception("Não foi encontrado um fornecedor com o id fornecido.");
            }
            try
            {
                product.Supplier = supplier;
                product.Description = model.Description;
                product.ManufactureDate = model.ManufactureDate;
                product.ExpirationDate = model.ExpirationDate;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                
                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao atualizar o produto.");
            }
        }

        public async Task<Product> RemoveAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new Exception("Não foi possível encontrar o produto para a deleção.");
            }
            
            try
            {
                product.IsActive = false;
                
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                
                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao deletar o produto.");
            }
        }
    }
}