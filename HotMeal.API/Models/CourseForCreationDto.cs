using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Models
{
    public class CourseForCreationDto
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Decimal Price { get; set; }
    }
}
