using Microsoft.EntityFrameworkCore;

namespace HotMeal.API.Entities
{

    public class HotMealContext : DbContext
    {
        public HotMealContext(DbContextOptions<HotMealContext> options)
           : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }

}
