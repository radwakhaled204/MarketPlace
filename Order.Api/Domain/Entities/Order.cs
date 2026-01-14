namespace Order.Api.Domain.Entities
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered
    }

    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; } 
        public int TotalItems { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public Guid CustomerId { get; set; }
        

        
    }

}
