using Microsoft.EntityFrameworkCore;

namespace User.service;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    { }
    public DbSet<User> Users { get; set; } = null!;
}