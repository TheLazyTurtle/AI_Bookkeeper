﻿// <auto-generated />
using System;
using DataManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataManager.Migrations
{
    [DbContext(typeof(DbContext))]
    partial class DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.1.23111.4");

            modelBuilder.Entity("DataManager.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DataManager.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategorySelection")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Counterparty")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("DebitCredit")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Notifications")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TransactionId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Date", "Account", "Counterparty", "DebitCredit", "Amount", "TransactionType", "Notifications")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DataManager.Models.Transaction", b =>
                {
                    b.HasOne("DataManager.Models.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DataManager.Models.Category", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
