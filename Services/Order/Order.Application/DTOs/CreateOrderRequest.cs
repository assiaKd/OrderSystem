

namespace Order.Application.DTOs
{
        public record CreateOrderRequest(
        List<CreateOrderItemRequest> Items
    );

        public record CreateOrderItemRequest(
            Guid ProductId,
            int Quantity
        );
    }
