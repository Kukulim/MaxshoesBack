﻿// <auto-generated />
using System;
using MaxshoesBack.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaxshoesBack.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210220144251_titleToNotificationModel")]
    partial class titleToNotificationModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
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
                            Id = "37846734-172e-4149-8cec-6f43d1eb3f60",
                            Email = "Employee1@test.pl",
                            IsEmailConfirmed = true,
                            Password = "$2a$11$nWN0OWn4fZGBf4T16CZW/ufntR/uBHn64jV4gCk/iZJW98FSNR0CS",
                            Role = "Employee",
                            UserName = "Employee1"
                        },
                        new
                        {
                            Id = "37846734-172e-4149-8cec-6f43d1eb3f61",
                            Email = "Employee2@test.pl",
                            IsEmailConfirmed = true,
                            Password = "$2a$11$aExTowF3IXre1V1Y7QLBb.Qcpf8xLdema/zB/be2PBEHypInyINm2",
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
                    b.OwnsOne("MaxshoesBack.Models.UserModels.Contact", "Contact", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("ApartmentNumber")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FirstName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("HouseNumber")
                                .HasColumnType("int");

                            b1.Property<string>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LastName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.HasData(
                                new
                                {
                                    UserId = "37846734-172e-4149-8cec-6f43d1eb3f60",
                                    ApartmentNumber = 23,
                                    City = "Czestochowa",
                                    FirstName = "Piter",
                                    HouseNumber = 45,
                                    Id = "d4afa636-7dbb-4e38-9d5f-d1c8f512ff6c",
                                    LastName = "Blukacz",
                                    PhoneNumber = "666111222",
                                    State = "Polska",
                                    Street = "Zielona",
                                    ZipCode = "42-200"
                                },
                                new
                                {
                                    UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                                    ApartmentNumber = 0,
                                    City = "Czestochowa",
                                    FirstName = "Jan",
                                    HouseNumber = 2,
                                    Id = "b528215d-4da8-4ab4-86fd-aada2f09bbd6",
                                    LastName = "Oko",
                                    PhoneNumber = "666111223",
                                    State = "Polska",
                                    Street = "Liliowa",
                                    ZipCode = "42-202"
                                });
                        });

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("MaxshoesBack.Models.UserModels.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
