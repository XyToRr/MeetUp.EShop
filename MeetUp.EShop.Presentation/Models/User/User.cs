using MeetUp.EShop.Presentation.Enums;
using System.Text.Json.Serialization;

namespace MeetUp.EShop.Presentation.Models.User
{
    public class User
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        public List<Order.Order>? Orders { get; set; }
        public UserRole? Role { get; set; }
    }
}
