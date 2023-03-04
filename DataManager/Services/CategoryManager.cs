using System.Collections.ObjectModel;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DataManager.Services;

public static class CategoryManager
{
    private static readonly DbContext Db = new();
    private static ObservableCollection<Category> Incoming { get; }
    private static ObservableCollection<Category> Expense { get; } 
    public static ObservableCollection<Category> Categories { get; }


    static CategoryManager()
    {
        Incoming = new ObservableCollection<Category>(Db.Categories.Where(c => c.Type == TransactionType.Income).ToList());
        Expense = new ObservableCollection<Category>(Db.Categories.Where(c => c.Type == TransactionType.Expense).ToList());
        Categories = new ObservableCollection<Category>(Db.Categories.ToList());
    }

    private static async Task UpdateCategory(Category category)
    {
        Db.Categories.Update(category);
        await Db.SaveChangesAsync();
        
        var old = Categories.First(c => c.Id == category.Id);

        if (Incoming.Contains(old) && old.Type != TransactionType.Income)
        {
            Incoming.Remove(old);
            Expense.Add(old);
            return;
        }
        if (Expense.Contains(old) && old.Type != TransactionType.Expense)
        {
            Expense.Remove(old);
            Incoming.Add(old);
        }
    }

    public static async Task AddOrUpdate(Category category)
    {
        if (!IsValidCategory(category))
            return;

        if (await Db.Categories.FirstOrDefaultAsync(c => c == category) is not null)
        {
            await UpdateCategory(category);
        }
        else
        {
            await AddCategory(category);
            AddToObservableList(category);
        }
    }
    
    public static Category? GetCategoryById(TransactionType type, int? id)
    {
        if (id == null)
            return null;
        
        if (type == TransactionType.Income)
            return Incoming.FirstOrDefault(c => c.Id == id);
        
        return Expense.FirstOrDefault(c => c.Id == id);
    }

    private static async Task AddCategory(Category category)
    {
        await Db.Categories.AddAsync(category);
        await Db.SaveChangesAsync();
        AddToObservableList(category);
    }

    public static async Task DeleteCategory(Category category)
    {
        Db.Categories.Remove(category);
        await Db.SaveChangesAsync();
        RemoveFromObservableList(category);
    }
    
    public static IEnumerable<Category> GetCategoriesByType(TransactionType type)
    {
        return type == TransactionType.Income ? Incoming : Expense;
    }

    private static bool IsValidCategory(Category category)
    {
        return category.Name is not "" and not null && category.Type is TransactionType.Expense or TransactionType.Income;
    }
    
    private static void AddToObservableList(Category category)
    {
        if (category.Type == TransactionType.Income)
        {
            if (!Incoming.Contains(category))
                Incoming.Add(category);
        }
        else
        {
            if (!Expense.Contains(category))
                Expense.Add(category);
        }
    }
    
    private static void RemoveFromObservableList(Category category)
    {
        if (category.Type == TransactionType.Income)
        {
            Incoming.Remove(category);
            return;
        }
        
        Expense.Remove(category);
    }
}