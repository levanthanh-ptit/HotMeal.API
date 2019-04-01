using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Models
{
    public class OrderItemDto
    {
        public CourseDto CourseDto { get; set; }
        public int Quantity { get; set; }
    }
}
