using Order.Domain.Enums;

namespace Order.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get;  set; }
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;
        public Order(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Pending;
        }

        public void AddItem(Guid productId, int quantity)
        {
            _items.Add(new OrderItem(productId, quantity));
        }
    }
}
