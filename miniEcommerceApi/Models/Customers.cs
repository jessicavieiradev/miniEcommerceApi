namespace miniEcommerceApi.Models
{
    public class Customers
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public Users User { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        public Customers() { }
        public Customers(Guid userId, string Name, string lastName, string phone, string cpf)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = Name;
            LastName = lastName;
            Phone = phone;
            Cpf = cpf;
        }
    }
}
