using MassTransit;
using Order.Application.DTOs;
using Order.Application.Interfaces;
using Order.Domain.Enums;
using OrderSystem.Contracts.DTOs;
using OrderSystem.Contracts.Events;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderService(IOrderRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task ConfirmOrder(string orderId, CancellationToken cancellationToken)
        {
            var order = await _repository.GetOrderByIdAsync(orderId, cancellationToken);

            order?.Status = OrderStatus.Confirmed;

            await _repository.SaveOrderAsync(order, cancellationToken);
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = new OrderEntity(Guid.NewGuid());

            foreach (var item in request.Items)
            {
                order.AddItem(item.ProductId, item.Quantity);
            }

            await _repository.SaveOrderAsync(order, cancellationToken);

            await _publishEndpoint.Publish(new OrderCreatedEvent(
            order.Id,
            request.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity)).ToList()), cancellationToken);
            return order.Id;
        }
    }
}
