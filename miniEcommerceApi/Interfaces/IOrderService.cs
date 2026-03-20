using miniEcommerceApi.DTOs.OrdersDTO.Request;
using miniEcommerceApi.DTOs.OrdersDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllOrders();
        Task<OrderResponse> GetOrderById(Guid id);
        Task<IEnumerable<OrderResponse>> GetOrdersByCustomerId(Guid customerId);
        Task<OrderResponse> CreateOrder(CreateOrderRequest dto);
        Task UpdateOrderStatus(Guid id, UpdateOrderStatusRequest dto);
        Task DeleteOrder(Guid id);
    }
}
