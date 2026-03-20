namespace miniEcommerceApi.DTOs.CustomersDTO.Response
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Cpf { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
