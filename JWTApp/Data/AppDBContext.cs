using Microsoft.EntityFrameworkCore;
using Models;

namespace JWTApp.Data
{
    public class AppDBContext : DbContext 
    {

        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
