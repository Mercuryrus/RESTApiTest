using Microsoft.EntityFrameworkCore;

namespace RESTApiTest.DbModels
{
    public class ApplicationContext : DbContext
    {

        public DbSet<BookDbModel> Books { get; set; }
        public DbSet<AuthorDbModel> Authors { get; set; }
        public DbSet<AuthorBooksDbModel> BookAuthor { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TaskEureka;Username=postgres;Password=Lollord9889");
        }
    }
}
