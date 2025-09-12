using MeetUp.EShop.Core.Models.Order;

namespace MeetUp.EShop.Core.Models.Client;

public class Client
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Adress { get; set; }
    public List<Order.Order> Orders { get; set; }
}