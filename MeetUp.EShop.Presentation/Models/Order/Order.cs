using MeetUp.EShop.Presentation.Enums;

namespace MeetUp.EShop.Presentation.Models.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }
    }
}