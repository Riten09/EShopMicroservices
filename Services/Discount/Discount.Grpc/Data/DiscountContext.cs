using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        // adding dbset for table creation name as Coupon
        public DbSet<Coupon> Coupones { get; set; } = default!;

        // creating constructor 
        public DiscountContext(DbContextOptions<DiscountContext> options)
            :base(options){
            
        }

        // seeding the data On Model creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(

                new Coupon { Id = 1, ProductName = "IPhone 16 pro max", Description = "This is Apple latest product", Amount = 999 },
                new Coupon { Id = 2, ProductName = "Samsung S25 Ultra", Description = "This is Samsung latest product", Amount = 1000 });
        }
    }
}
