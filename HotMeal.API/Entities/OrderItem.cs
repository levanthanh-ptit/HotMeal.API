using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Entities
{
    public class OrderItem
    {
        [Key]
        [ForeignKey("OrderList")]
        public Guid CourseId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
