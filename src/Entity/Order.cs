using System.Text.Json.Serialization;

namespace src.Entity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatuses
    {
        Pending,
        Shipped,
        Delivered,
    }

    public class Order
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
