using miniEcommerceApi.Enums;
using miniEcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests
{
    public static class OrderBuilder
    {
        public static Orders Default() => new Orders
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Status = OrderStatus.Pending,
            TotalAmount = 100m,
            ZipCode = "00000-000",
            Street = "Rua Teste",
            Number = "1",
            Neighborhood = "Bairro",
            City = "Cidade",
            State = "SP"
        };
    }
}
