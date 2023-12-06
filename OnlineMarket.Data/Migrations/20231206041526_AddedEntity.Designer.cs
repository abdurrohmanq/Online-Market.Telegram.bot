﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OnlineMarket.Data.DbContexts;

#nullable disable

namespace OnlineMarket.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231206041526_AddedEntity")]
    partial class AddedEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Carts.Cart", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Carts.CartItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("CartId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("Sum")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Categories.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2517),
                            Description = "Category description 1",
                            IsDelete = false,
                            Name = "FastFood",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2523),
                            Description = "Category description 2",
                            IsDelete = false,
                            Name = "Ichimliklar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2524),
                            Description = "Category description 3",
                            IsDelete = false,
                            Name = "Kaboblar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2525),
                            Description = "Category description 4",
                            IsDelete = false,
                            Name = "Suyuq ovqatlar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2526),
                            Description = "Category description 5",
                            IsDelete = false,
                            Name = "Go'shtli ovqatlar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2535),
                            Description = "Category description 6",
                            IsDelete = false,
                            Name = "Shashliklar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2535),
                            Description = "Category description 7",
                            IsDelete = false,
                            Name = "Salatlar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2536),
                            Description = "Category description 8",
                            IsDelete = false,
                            Name = "Xamirli ovqatlar",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Filials.Filial", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Filials");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2574),
                            IsDelete = false,
                            Location = "Novza",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2576),
                            IsDelete = false,
                            Location = "Chilonzor",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2577),
                            IsDelete = false,
                            Location = "Sergeli",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2577),
                            IsDelete = false,
                            Location = "Oq-tepa",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Orders.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("CartId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("MarketAddress")
                        .HasColumnType("text");

                    b.Property<int>("OrderType")
                        .HasColumnType("integer");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Products.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CategoryId = 1L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2539),
                            Description = "Product Desc",
                            IsDelete = false,
                            Name = "HotDog",
                            Price = 10.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2L,
                            CategoryId = 1L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2546),
                            Description = "Product Desc2",
                            IsDelete = false,
                            Name = "Gamburger",
                            Price = 15.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3L,
                            CategoryId = 1L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2548),
                            Description = "Product Desc3",
                            IsDelete = false,
                            Name = "Lavash",
                            Price = 10.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4L,
                            CategoryId = 1L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2549),
                            Description = "Product Desc4",
                            IsDelete = false,
                            Name = "Non burger",
                            Price = 15.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5L,
                            CategoryId = 2L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2550),
                            Description = "Product Desc5",
                            IsDelete = false,
                            Name = "Coco-Cola",
                            Price = 20.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6L,
                            CategoryId = 2L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2552),
                            Description = "Product Desc6",
                            IsDelete = false,
                            Name = "Pepsi",
                            Price = 25.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 7L,
                            CategoryId = 2L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2554),
                            Description = "Product Desc7",
                            IsDelete = false,
                            Name = "Fanta",
                            Price = 20.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 8L,
                            CategoryId = 2L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2555),
                            Description = "Product Desc8",
                            IsDelete = false,
                            Name = "Kompot",
                            Price = 25.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 9L,
                            CategoryId = 3L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2556),
                            Description = "Product Desc5",
                            IsDelete = false,
                            Name = "Tovuq kabob",
                            Price = 20.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 10L,
                            CategoryId = 3L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2559),
                            Description = "Product Desc6",
                            IsDelete = false,
                            Name = "Lo'la kabob",
                            Price = 25.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 11L,
                            CategoryId = 3L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2560),
                            Description = "Product Desc5",
                            IsDelete = false,
                            Name = "Mol kabob",
                            Price = 20.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 12L,
                            CategoryId = 3L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2561),
                            Description = "Product Desc6",
                            IsDelete = false,
                            Name = "Qo'y kabob",
                            Price = 28.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 13L,
                            CategoryId = 4L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2562),
                            Description = "Product Desc13",
                            IsDelete = false,
                            Name = "Sho'rva",
                            Price = 30.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 14L,
                            CategoryId = 4L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2564),
                            Description = "Product Desc14",
                            IsDelete = false,
                            Name = "Mastava",
                            Price = 28.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 15L,
                            CategoryId = 5L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2565),
                            Description = "Product Desc13",
                            IsDelete = false,
                            Name = "Qozon kabob",
                            Price = 30.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 16L,
                            CategoryId = 5L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2567),
                            Description = "Product Desc14",
                            IsDelete = false,
                            Name = "Tovuq go'shti",
                            Price = 28.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 17L,
                            CategoryId = 6L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2568),
                            Description = "Product Desc13",
                            IsDelete = false,
                            Name = "Qiyma shashlik",
                            Price = 30.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 18L,
                            CategoryId = 6L,
                            CreatedAt = new DateTime(2023, 12, 6, 4, 15, 26, 367, DateTimeKind.Utc).AddTicks(2570),
                            Description = "Product Desc14",
                            IsDelete = false,
                            Name = "Burda shashlik",
                            Price = 18.0m,
                            StockQuantity = 10,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Carts.Cart", b =>
                {
                    b.HasOne("OnlineMarket.Domain.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Carts.CartItem", b =>
                {
                    b.HasOne("OnlineMarket.Domain.Entities.Carts.Cart", "Cart")
                        .WithMany("Items")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlineMarket.Domain.Entities.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Orders.Order", b =>
                {
                    b.HasOne("OnlineMarket.Domain.Entities.Carts.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("CartId");

                    b.HasOne("OnlineMarket.Domain.Entities.Users.User", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.OwnsOne("Telegram.Bot.Types.Location", "DeliveryAddress", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.Property<int?>("Heading")
                                .HasColumnType("integer");

                            b1.Property<float?>("HorizontalAccuracy")
                                .HasColumnType("real");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision");

                            b1.Property<int?>("LivePeriod")
                                .HasColumnType("integer");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.Property<int?>("ProximityAlertRadius")
                                .HasColumnType("integer");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Cart");

                    b.Navigation("DeliveryAddress");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Products.Product", b =>
                {
                    b.HasOne("OnlineMarket.Domain.Entities.Categories.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Carts.Cart", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Categories.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("OnlineMarket.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
