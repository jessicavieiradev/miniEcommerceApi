using miniEcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class OrderItemBuilder
    {
        public static OrderItem Default(Guid orderId, Guid productId, int quantity = 2) => new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = 50m
        };
    }
}
