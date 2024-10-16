using Microsoft.EntityFrameworkCore;
using TodoListApi.Models;

public class AppDbContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
