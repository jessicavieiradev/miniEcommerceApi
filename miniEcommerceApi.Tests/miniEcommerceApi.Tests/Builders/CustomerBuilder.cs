using miniEcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class CustomerBuilder
    {
        public static Customers Default(Guid userId) => new Customers
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FirstName = "May", 
            LastName = "Tae",
            Phone = "1234567890",
            Cpf = "12345678901"
        };
    }
}
