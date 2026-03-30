using OrderSystem.Contracts.DTOs;

namespace OrderSystem.Contracts.Events
{
    public record OrderCreatedEvent(
     Guid OrderId,
     List<OrderItemDto> Items
 );

}
