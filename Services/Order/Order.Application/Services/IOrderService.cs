
using Order.Application.DTOs;

namespace Order.Application.Services
{
    public interface IOrderService
    {
        Task ConfirmOrder(string orderId, CancellationToken cancellationToken);
        Task<Guid> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    }
}
