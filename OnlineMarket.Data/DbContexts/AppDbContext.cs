using Microsoft.EntityFrameworkCore;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Categories;
using OnlineMarket.Domain.Entities.Filials;
using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Domain.Entities.Users;

namespace OnlineMarket.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Filial> Filials { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(u => u.User)
            .WithMany(u => u.Orders) 
            .HasForeignKey(u => u.UserId)   
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(o => o.Order)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        SetData(modelBuilder);
    }

    private void SetData(ModelBuilder modelBuilder)
    {
        var categoryDataList = new List<Category>
    {
        new Category
        {
            Id = 1,
            Name = "FastFood",
            Description = "Category description 1"
        },
        new Category
        {
            Id = 2,
            Name = "Ichimliklar",
            Description = "Category description 2"
        },
        new Category
        {
            Id = 3,
            Name = "Kaboblar",
            Description = "Category description 3"
        },

        new Category
        {
            Id = 4,
            Name = "Suyuq ovqatlar",
            Description = "Category description 4"
        },
        new Category
        {
            Id = 5,
            Name = "Go'shtli ovqatlar",
            Description = "Category description 5"
        },
        new Category
        {
            Id = 6,
            Name = "Shashliklar",
            Description = "Category description 6"
        },
        new Category
        {
            Id = 7,
            Name = "Salatlar",
            Description = "Category description 7"
        },
        new Category
        {
            Id = 8,
            Name = "Xamirli ovqatlar",
            Description = "Category description 8"
        },
    };

        var productDataList = new List<Product>
    {
        new Product
        {
            Id = 1,
            Name = "HotDog",
            Description ="Product Desc",
            Price = 10.0m,
            StockQuantity = 10,
            CategoryId = 1
        },
        new Product
        {
            Id = 2,
            Name = "Gamburger",
            Description ="Product Desc2",
            Price = 15.0m,
            StockQuantity = 10,
            CategoryId = 1
        },
        new Product
        {
            Id = 3,
            Name = "Lavash",
            Description ="Product Desc3",
            Price = 10.0m,
            StockQuantity = 10,
            CategoryId = 1
        },
        new Product
        {
            Id = 4,
            Name = "Non burger",
            Description ="Product Desc4",
            Price = 15.0m,
            StockQuantity = 10,
            CategoryId = 1
        },
        new Product
        {
            Id = 5,
            Name = "Coco-Cola",
            Description ="Product Desc5",
            Price = 20.0m,
            StockQuantity = 10,
            CategoryId = 2
        },
        new Product
        {
            Id = 6,
            Name = "Pepsi",
            Description ="Product Desc6",
            Price = 25.0m,
            StockQuantity = 10,
            CategoryId = 2
        },
        new Product
        {
            Id = 7,
            Name = "Fanta",
            Description ="Product Desc7",
            Price = 20.0m,
            StockQuantity = 10,
            CategoryId = 2
        },
        new Product
        {
            Id = 8,
            Name = "Kompot",
            Description ="Product Desc8",
            Price = 25.0m,
            StockQuantity = 10,
            CategoryId = 2
        },
        new Product
        {
            Id = 9,
            Name = "Tovuq kabob",
            Description ="Product Desc5",
            Price = 20.0m,
            StockQuantity = 10,
            CategoryId = 3
        },
        new Product
        {
            Id = 10,
            Name = "Lo'la kabob",
            Description ="Product Desc6",
            Price = 25.0m,
            StockQuantity = 10,
            CategoryId = 3
        },
        new Product
        {
            Id = 11,
            Name = "Mol kabob",
            Description ="Product Desc5",
            Price = 20.0m,
            StockQuantity = 10,
            CategoryId = 3
        },
        new Product
        {
            Id = 12,
            Name = "Qo'y kabob",
            Description ="Product Desc6",
            Price = 28.0m,
            StockQuantity = 10,
            CategoryId = 3
        },
        new Product
        {
            Id = 13,
            Name = "Sho'rva",
            Description ="Product Desc13",
            Price = 30.0m,
            StockQuantity = 10,
            CategoryId = 4
        },
        new Product
        {
            Id = 14,
            Name = "Mastava",
            Description ="Product Desc14",
            Price = 28.0m,
            StockQuantity = 10,
            CategoryId = 4
        },

        new Product
        {
            Id = 15,
            Name = "Qozon kabob",
            Description ="Product Desc13",
            Price = 30.0m,
            StockQuantity = 10,
            CategoryId = 5
        },
        new Product
        {
            Id = 16,
            Name = "Tovuq go'shti",
            Description ="Product Desc14",
            Price = 28.0m,
            StockQuantity = 10,
            CategoryId = 5
        },
        new Product
        {
            Id = 17,
            Name = "Qiyma shashlik",
            Description ="Product Desc13",
            Price = 30.0m,
            StockQuantity = 10,
            CategoryId = 6
        },
        new Product
        {
            Id = 18,
            Name = "Burda shashlik",
            Description ="Product Desc14",
            Price = 18.0m,
            StockQuantity = 10,
            CategoryId = 6
        }
    };

        var location = new List<Filial>()
        {
            new Filial { Id = 1, Location = "Novza"},
            new Filial { Id = 2, Location = "Chilonzor"},
            new Filial { Id = 3, Location = "Sergeli"},
            new Filial { Id = 4, Location = "Oq-tepa"},
        };


        modelBuilder.Entity<Category>().HasData(categoryDataList);
        modelBuilder.Entity<Product>().HasData(productDataList);
        modelBuilder.Entity<Filial>().HasData(location);
    }
}
