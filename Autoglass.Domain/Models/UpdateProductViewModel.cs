using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Autoglass.Domain.Models
{
    public class UpdateProductViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "O id do produto é obrigatório.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        public string Description { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int SupplierId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(DateTime.Compare(ManufactureDate, ExpirationDate) < 0))
            {
                yield return new ValidationResult("A data de fabricação não pode ser maior ou igual à data de validade.");
            }
        }
    }
}