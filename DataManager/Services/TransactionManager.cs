using System.Collections.ObjectModel;
using DataManager.Models;

namespace DataManager.Services;

public static class TransactionManager
{
    private static readonly DbContext Db = new();
    public static ObservableCollection<Transaction> Transactions { get; }

    static TransactionManager()
    {
        Transactions = new ObservableCollection<Transaction>(GetTransactions());
    }

    public static async Task UpdateTransaction(Transaction transaction)
    {
        Db.Transactions.Update(transaction);
        await Db.SaveChangesAsync();
    }
    
    public static async Task AddTransactions(IEnumerable<Transaction> transactions)
    {
        var res = transactions.Where(transaction => !Transactions.Any(t => t.Equals(transaction))).ToList();

        await Db.Transactions.AddRangeAsync(res);
        await Db.SaveChangesAsync();

        foreach (var transaction in res)
        {
            Transactions.Add(transaction);
        }
    }

    private static IEnumerable<Transaction> GetTransactions()
    {
        var data = Db.Transactions;
        
        foreach (var transaction in data)
        {
            transaction.Category = CategoryManager.GetCategoryById(transaction.DebitCredit, transaction.CategoryId);
        }
        
        return data;
    }
}