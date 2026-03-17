namespace miniEcommerceApi.DTOs.CostumersDTO.Response
{
    public class CostumerResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
