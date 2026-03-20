using miniEcommerceApi.DTOs.PaymentDTO.Request;
using miniEcommerceApi.DTOs.PaymentDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPayment(CreatePaymentRequest dto);
        Task<PaymentResponse> GetPaymentByOrderId(Guid orderId);
    }
}
