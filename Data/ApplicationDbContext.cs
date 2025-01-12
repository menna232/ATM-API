using Microsoft.EntityFrameworkCore;

namespace ATM_Api.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }

        public DbSet<Customer> customers { get; set; }
        public DbSet<TransactionInfo> transactions { get; set; }    
        public DbSet<Operations> operations { get; set; }   

    }
}
