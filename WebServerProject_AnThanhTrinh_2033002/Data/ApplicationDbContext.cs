using Microsoft.EntityFrameworkCore;
using WebServerProject_AnThanhTrinh_2033002.Models;
using Task = WebServerProject_AnThanhTrinh_2033002.Models.Task;

namespace WebServerProject_AnThanhTrinh_2033002.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

    }
}
