using miniEcommerceApi.DTOs.PaymentDTO.Request;
using miniEcommerceApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Builders
{
    public static class PaymentRequestBuilder
    {
        public static CreatePaymentRequest Default(Guid orderId) => new CreatePaymentRequest
        {
            OrderId = orderId,
            Method = PaymentMethod.Pix
        };

        public static CreatePaymentRequest WithInvalidOrder() => new CreatePaymentRequest
        {
            OrderId = Guid.NewGuid(),
            Method = PaymentMethod.Pix
        };
    }
}
