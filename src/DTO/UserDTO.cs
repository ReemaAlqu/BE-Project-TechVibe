using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using src.Entity;
using src.Utils;
using static src.Entity.User;

namespace src.DTO
{
    public class UserDTO
    {
        public class UserCreateDto
        {
            public string? Name { get; set; }

            [Required]
            [PasswordComplexity]
            public string Password { get; set; }

            [Required]
            [EmailAddress(ErrorMessage = "Please provide email with right format")]
            public string EmailAddress { get; set; }
            public string? Phone { get; set; }
        }

        public class UserSignInDto
        {
            [Required]
            [EmailAddress(ErrorMessage = "Please provide email with right format")]
            public string EmailAddress { get; set; }

            [Required]
            [PasswordComplexity]
            public string Password { get; set; }
        }

        public class UserReadDto
        {
            public Guid UserID { get; set; }
            public string? Name { get; set; }
            public string? EmailAddress { get; set; }
            public string? Password { get; set; }
            public string? Phone { get; set; }
            public Role UserRole { get; set; }
        }

        public class UserUpdateDto
        {
            public string? Name { get; set; }
            public string? EmailAddress { get; set; }
            public string? Password { get; set; }
            public string? Phone { get; set; }
        }
    }
}
