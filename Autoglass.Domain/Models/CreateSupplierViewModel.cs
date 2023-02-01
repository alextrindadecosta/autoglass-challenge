using System.ComponentModel.DataAnnotations;

namespace Autoglass.Domain.Models
{
    public class CreateSupplierViewModel
    {
        [Required(ErrorMessage = "A descrição do fornecedor é obrigatória.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "O CNPJ do fornecedor é obrigatório.")]
        public string CNPJ { get; set; }
    }
}