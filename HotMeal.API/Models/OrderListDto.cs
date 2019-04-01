using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Models
{
    public class OrderListDto
    {
       
        public Guid Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public Guid CustomerId { get; set; }

        public ICollection<OrderItemDto> OrderItemsDto { get; set; } = new List<OrderItemDto>();
    }
}
