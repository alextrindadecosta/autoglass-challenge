namespace Autoglass.Domain.Entities
{
    public class Supplier
    {
        public Supplier() {}
        public Supplier(string description, string cnpj)
        {
            Description = description;
            CNPJ = cnpj;
        }
        
        public int Id { get; set; }
        public string Description { get; set; }
        public string CNPJ { get; set; }
    }
}