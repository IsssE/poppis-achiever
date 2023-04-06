﻿using Microsoft.EntityFrameworkCore;
using Authentication.Models;

namespace Authentication.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    { }

    public DbSet<UserAuthDetails> Users { get; set; } = null!;
}
