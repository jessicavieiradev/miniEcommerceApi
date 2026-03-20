namespace miniEcommerceApi.DTOs.AddressDTO.Response
{
    public class AddressResponse
    {
        public Guid Id { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public AddressResponse() { }
        public AddressResponse(Guid id, string zipCode, string street, string number, string neighborhood, string city, string state)
        {
            Id = id;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
}
