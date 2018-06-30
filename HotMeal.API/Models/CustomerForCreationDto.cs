using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Models
{
    public class CustomerForCreationDto
    {
        public string Name { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Genre { get; set; }
    }
}
