using Microsoft.EntityFrameworkCore;
using Purchases_Calculator.API.Domain;
using System.Collections.Generic;

namespace Purchases_Calculator.API.Infrastructure.Databases;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Purchase> Purchases { get; set; }
}
