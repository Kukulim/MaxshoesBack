﻿// <auto-generated />
using System;
using MaxshoesBack.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaxshoesBack.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MaxshoesBack.Models.UserModels.Notification", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("MyProperty")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("MaxshoesBack.Models.UserModels.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "c006ff84-293a-43eb-ad73-e999f7cef947",
                            Email = "Employee1@test.pl",
                            IsEmailConfirmed = true,
                            Password = "$2a$11$AvQ1DXZUXF6ZfOb6Q3R.7.fCWtFBx3lvwWY1ReaNGSCaN9OMouZ26",
                            Role = "Employee",
                            UserName = "Employee1"
                        },
                        new
                        {
                            Id = "5052f1c4-c16c-461e-99c9-270ffba33e13",
                            Email = "Employee2@test.pl",
                            IsEmailConfirmed = true,
                            Password = "$2a$11$wHex25HW6vYlMflv0zAfWO90Bom7BmIMurunUakHTB/sILKftxnxu",
                            Role = "Employee",
                            UserName = "Employee2"
                        });
                });

            modelBuilder.Entity("MaxshoesBack.Models.UserModels.Notification", b =>
                {
                    b.HasOne("MaxshoesBack.Models.UserModels.User", null)
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MaxshoesBack.Models.UserModels.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
