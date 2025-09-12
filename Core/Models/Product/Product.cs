using System.Text.Json.Serialization;

namespace MeetUp.EShop.Core.Models.Product
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get;  set; }
        public int Price { get; set; }

        [JsonIgnore]
        public List<Order.Order> Orders { get; set; }
    }
}