using System.Data.Entity;

namespace Calendar
{
    public class UserContext : DbContext
    {
        public DbSet<Calendar> Calendar { get; set; }

        public UserContext() : base("name=MyDbContext")
        {
        }
    }
}