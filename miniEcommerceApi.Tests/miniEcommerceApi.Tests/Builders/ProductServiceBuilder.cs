using miniEcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class ProductBuilder
    {
        public static Products Default() => new Products(
            name: "Produto Teste",
            description: "Descrição Teste",
            price: 50m,
            stock: 10,
            imageUrl: "http://teste.com/img.jpg",
            categoryId: Guid.NewGuid(),
            IsActive: true
        );
    }
}
