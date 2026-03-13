namespace miniEcommerceApi.Models
{
    public class Costumers
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Users User { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}
