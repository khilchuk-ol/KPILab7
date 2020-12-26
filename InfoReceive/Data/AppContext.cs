
using Microsoft.EntityFrameworkCore;

namespace InfoReceive
{
    public class AppContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database = InfoRabbitDB; Trusted_Connection = True;");
        }
    }
}
