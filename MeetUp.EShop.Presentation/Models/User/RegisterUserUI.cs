using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeetUp.EShop.Presentation.Models.User
{
    public class RegisterUserUI
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
        [JsonPropertyName("firstName")]
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
