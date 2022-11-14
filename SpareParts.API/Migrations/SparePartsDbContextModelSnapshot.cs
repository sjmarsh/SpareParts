﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpareParts.API.Data;

#nullable disable

namespace SpareParts.API.Migrations
{
    [DbContext(typeof(SparePartsDbContext))]
    partial class SparePartsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SpareParts.API.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("SpareParts.API.Entities.InventoryItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateRecorded")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PartID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PartID");

                    b.ToTable("InventoryItems");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            DateRecorded = new DateTime(2022, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6966),
                            PartID = 1,
                            Quantity = 11
                        },
                        new
                        {
                            ID = 2,
                            DateRecorded = new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7130),
                            PartID = 1,
                            Quantity = 13
                        },
                        new
                        {
                            ID = 3,
                            DateRecorded = new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7143),
                            PartID = 1,
                            Quantity = 5
                        },
                        new
                        {
                            ID = 4,
                            DateRecorded = new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7153),
                            PartID = 2,
                            Quantity = 22
                        },
                        new
                        {
                            ID = 5,
                            DateRecorded = new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7164),
                            PartID = 2,
                            Quantity = 16
                        },
                        new
                        {
                            ID = 6,
                            DateRecorded = new DateTime(2022, 8, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7178),
                            PartID = 2,
                            Quantity = 1
                        },
                        new
                        {
                            ID = 7,
                            DateRecorded = new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7188),
                            PartID = 3,
                            Quantity = 33
                        },
                        new
                        {
                            ID = 8,
                            DateRecorded = new DateTime(2022, 8, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7199),
                            PartID = 3,
                            Quantity = 50
                        },
                        new
                        {
                            ID = 9,
                            DateRecorded = new DateTime(2022, 9, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(7209),
                            PartID = 3,
                            Quantity = 40
                        });
                });

            modelBuilder.Entity("SpareParts.API.Entities.Part", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.ToTable("Parts");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Description = "The first part",
                            Name = "Part 1",
                            Price = 10.1,
                            StartDate = new DateTime(2022, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6356),
                            Weight = 1.1100000000000001
                        },
                        new
                        {
                            ID = 2,
                            Description = "The second part",
                            Name = "Part 2",
                            Price = 12.220000000000001,
                            StartDate = new DateTime(2022, 6, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6546),
                            Weight = 2.2200000000000002
                        },
                        new
                        {
                            ID = 3,
                            Description = "The third part",
                            EndDate = new DateTime(2023, 5, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6562),
                            Name = "Part 3",
                            Price = 13.300000000000001,
                            StartDate = new DateTime(2022, 7, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6560),
                            Weight = 3.3300000000000001
                        },
                        new
                        {
                            ID = 4,
                            Description = "The fourth part",
                            EndDate = new DateTime(2022, 10, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6578),
                            Name = "Part 4",
                            Price = 14.4,
                            StartDate = new DateTime(2021, 11, 14, 11, 19, 57, 0, DateTimeKind.Local).AddTicks(6576),
                            Weight = 4.4400000000000004
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SpareParts.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SpareParts.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SpareParts.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SpareParts.API.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SpareParts.API.Entities.InventoryItem", b =>
                {
                    b.HasOne("SpareParts.API.Entities.Part", null)
                        .WithMany()
                        .HasForeignKey("PartID");
                });
#pragma warning restore 612, 618
        }
    }
}
