namespace OrderSystem.Contracts.DTOs
{
    public record OrderItemDto(Guid ProductId, int Quantity);
}
