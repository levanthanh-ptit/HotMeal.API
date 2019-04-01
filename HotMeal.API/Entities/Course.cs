using System;
using System.ComponentModel.DataAnnotations;

namespace HotMeal.API.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        [MaxLength(200)]
        public String Description { get; set; }

        [Required]
        public Decimal Price { get; set; }
    }
}
