using MeetUp.EShop.Core.Models.Client;
using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Models.User;
using System.Text.Json.Serialization;

namespace MeetUp.EShop.Core.Models.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }

        [JsonIgnore]
        public User.User? User { get; set; }
        public List<Product.Product> Products { get; set; } = new List<Product.Product>();
    }
}