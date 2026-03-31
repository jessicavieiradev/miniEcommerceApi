using miniEcommerceApi.DTOs.CustomersDTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class CreateCustomerRequestBuilder
    {
        public static CreateCustomerRequest Default() => new CreateCustomerRequest
        {
            Cpf = "12345678901",
            Email = "john@example.com",
            Password = "Test1234!",
            Name = "John",
            LastName = "Doe",
            Username = "john_doe",
            Phone = "5511999999999"
        };
    }
}
