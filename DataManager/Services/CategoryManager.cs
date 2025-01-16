using System.Collections.ObjectModel;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DataManager.Services;

public class CategoryManager
{
    private static readonly DbContext Db = new();
    public static ObservableCollection<Category> Incoming { get; }
    public static ObservableCollection<Category> Expense { get; } 
    public static ObservableCollection<Category> Categories { get; }

    static CategoryManager()
    {
        Incoming = new ObservableCollection<Category>(Db.Categories.Where(c => c.Type == TransactionType.Income).ToList());
        Expense = new ObservableCollection<Category>(Db.Categories.Where(c => c.Type == TransactionType.Expense).ToList());
        Categories = new ObservableCollection<Category>(Db.Categories.ToList());
    }

    public static async Task AddOrUpdate(Category category)
    {
        if (!IsValidCategory(category))
            return;

        if (await Db.Categories.FirstOrDefaultAsync(c => c == category) != null)
            Db.Categories.Update(category);
        else
            Db.Categories.Add(category);
        
        await Db.SaveChangesAsync();
    }
    
    public static Category? GetCategoryById(TransactionType type, int? id)
    {
        if (id == null)
            return null;
        
        if (type == TransactionType.Income)
            return Incoming.FirstOrDefault(c => c.Id == id);
        
        return Expense.FirstOrDefault(c => c.Id == id);
    }

    public static async Task DeleteCategory(Category category)
    {
        if (Db.Categories.FirstOrDefault(c => c.Id == category.Id) == null)
            return;
        
        Db.Categories.Remove(category);
        await Db.SaveChangesAsync();
    }
    
    public static IEnumerable<Category> GetCategoriesByType(TransactionType type)
    {
        return type == TransactionType.Income ? Incoming : Expense;
    }

    private static bool IsValidCategory(Category category)
    {
        return category.Name is not "" and not null && category.Type is TransactionType.Expense or TransactionType.Income;
    }
}