using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using src.DTO;
using src.Utils;

namespace src.Entity
{
    public class User
    {
        public Guid UserID { get; set; }
        public string? Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please provide email with right format")]
        public string EmailAddress { get; set; }

        [Required]
        [PasswordComplexity]
        public string Password { get; set; }

        public byte[]? Salt { get; set; }
        public string? Phone { get; set; }
        public Role UserRole { get; set; } = Role.Customer;

        // ********************************************************************************

        public ICollection<Order> Orders { get; set; }

        public static implicit operator User(UserDTO.UserCreateDto v)
        {
            throw new NotImplementedException();
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Role
        {
            Admin,
            Customer,
        }
    }
}
