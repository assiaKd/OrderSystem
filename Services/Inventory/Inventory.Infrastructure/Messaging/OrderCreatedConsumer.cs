using Inventory.Application.DTOs;
using Inventory.Application.services;
using MassTransit;
using OrderSystem.Contracts.Events;

namespace Inventory.Infrastructure.Messaging
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IInventoryService _inventoryService;

        public OrderCreatedConsumer(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var items = context.Message.Items.Select(i => new InventoryItemDto
            (
               i.ProductId,
               i.Quantity
            ));

            await _inventoryService.ReserveStockAsync(items, context.CancellationToken);
        }
    }
}
