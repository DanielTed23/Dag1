using Microsoft.EntityFrameworkCore;

namespace Dag1.Data.Todo;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

    public DbSet<Cpr> Cprs { get; set; }
    public DbSet<Todolist> Todolists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Navngiv tabeller præcist som du ønsker dem
        modelBuilder.Entity<Cpr>().ToTable("Cpr");
        modelBuilder.Entity<Todolist>().ToTable("Todolist");

        // Definér relationen
        modelBuilder.Entity<Todolist>()
            .HasOne(t => t.User)
            .WithMany(c => c.Todolists)
            .HasForeignKey(t => t.UserId);
    }
}
