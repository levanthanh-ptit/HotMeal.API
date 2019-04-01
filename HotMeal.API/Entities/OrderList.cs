using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Entities
{
    public class OrderList
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
