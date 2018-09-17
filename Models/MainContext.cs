using Microsoft.EntityFrameworkCore;
 
namespace LoginReg.Models
{
    public class MainContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }
        public DbSet<User> user { get; set; }
        public DbSet<Wedding> wedding { get; set; }
        public DbSet<Rsvp> rsvp { get; set; }
    }
}