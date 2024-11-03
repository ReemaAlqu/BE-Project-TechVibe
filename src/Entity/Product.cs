using System.ComponentModel.DataAnnotations;

namespace src.Entity
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public required decimal Price { get; set; }

        [MaxLength(250), MinLength(5)]
        [RegularExpression(
            @"^[a-zA-Z0-9\s]+$",
            ErrorMessage = "Description must contain only letters, numbers ."
        )]
        public required string Description { get; set; }
        public Guid CategoryId { get; set; }
        public required Category Category { get; set; }

        /**************************************************************/
        public required string ImageUrl { get; set; }

        internal static decimal Sum(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
