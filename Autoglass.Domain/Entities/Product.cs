using System;
namespace Autoglass.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string description, 
            bool isActive, 
            DateTime manufactureDate, 
            DateTime expirationDate,
            int supplierId)
        {
            Description = description;
            IsActive = isActive;
            ManufactureDate = manufactureDate;
            ExpirationDate = expirationDate;
            SupplierId = supplierId;
        }
        
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
    }
}
