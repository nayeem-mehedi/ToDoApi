using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Data;

public class TodoDbContext : DbContext 
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) :  base(options){}
    
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
    public DbSet<User> Users => Set<User>();
}