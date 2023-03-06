using DataManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DataManager.Services;

public static class ReportManager
{
    private static readonly DbContext Db = new();
    
    public static IEnumerable<Report> GetCategoriesWithTotalAmount()
    {
        var categoriesWithTransactions = Db.Categories.Include(c => c.Transactions);
        var reports = categoriesWithTransactions.Select(c => new Report {
            Name = c.Name,
            Amount = c.Transactions.Sum(t => t.Amount),
            Type = c.Type
        }).ToList();

        return reports;
    }
    
    public static IEnumerable<Report> GetTotalAmountForTransactionType()
    {
        var reports = GetCategoriesWithTotalAmount().ToList();
        var income = new Report
        {
            Name = "Total",
            Amount = reports.Where(r => r.Type == TransactionType.Income).Sum(r => r.Amount),
            Type = TransactionType.Income
        };
        var expense = new Report
        {
            Name = "Total",
            Amount = reports.Where(r => r.Type == TransactionType.Expense).Sum(r => r.Amount),
            Type = TransactionType.Expense
        };
        
        return new List<Report>{income, expense};
    }
}