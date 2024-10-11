using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace QuizApp.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookModel> Books { get; set; }
    }
}
