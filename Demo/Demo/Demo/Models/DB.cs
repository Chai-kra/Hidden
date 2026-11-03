using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

#nullable disable warnings

public class DB(DbContextOptions options) : DbContext(options)
{
    // DbSets
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}

// Entity Classes -------------------------------------------------------------

public class Category
{
    [Key, MaxLength(4)]
    public string Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }

    public List<Product> Products { get; set; } = [];
}

public class Product
{
    [Key, MaxLength(4)]
    public string Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [Precision(4, 2)]
    public decimal Price { get; set; }

    public string CategoryId { get; set; }

    public Category Category { get; set; }
}
