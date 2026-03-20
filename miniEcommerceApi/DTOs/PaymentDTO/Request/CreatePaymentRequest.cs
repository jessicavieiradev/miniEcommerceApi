using miniEcommerceApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.PaymentDTO.Request
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "OrderId is required")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [EnumDataType(typeof(PaymentMethod), ErrorMessage = "Invalid payment method")]
        public PaymentMethod Method { get; set; }
    }
}
