using DataManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DataManager;

internal sealed class DbContext: Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public string DbPath { get; }

    public DbContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "transactions.sqlite");
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Such a big unique is needed because else you can incorrectly flag duplicates
        builder.Entity<Transaction>()
            .HasIndex(t => new {t.Date, t.Account, t.Counterparty, t.DebitCredit, t.Amount, t.TransactionType, t.Notifications})
            .IsUnique();

        builder.Entity<Transaction>()
            .HasOne<Category>(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId);
        
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}