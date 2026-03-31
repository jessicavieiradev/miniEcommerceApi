namespace miniEcommerceApi.Models
{
    public class Customers
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Users User { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        public Customers() { }
        public Customers(Guid userId, string name, string lastName, string phone, string cpf)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            FirstName = name;
            LastName = lastName;
            Phone = phone;
            Cpf = cpf;
        }
    }
}
