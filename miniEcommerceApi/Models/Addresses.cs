namespace miniEcommerceApi.Models
{
    public class Addresses
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Customers Customer { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
