namespace miniEcommerceApi.Models
{
    public class Adresses
    {
        public Guid Id { get; set; }
        public Guid CostumerId { get; set; }
        public Costumers Costumer { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Neiborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
