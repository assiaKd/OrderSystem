using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderEntity?> GetOrderByIdAsync(string id, CancellationToken cancellationToken);
        Task SaveOrderAsync(OrderEntity order, CancellationToken cancellationToken);
    }
}
