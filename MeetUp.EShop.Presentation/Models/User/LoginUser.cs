using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeetUp.EShop.Presentation.Models.User
{
    public class LoginUser
    {
        [JsonPropertyName("login")]
        [Required(ErrorMessage = "Login is required")]
        [StringLength(50, ErrorMessage = "Login must be less than 50 characters")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Password must be less than 50 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}
