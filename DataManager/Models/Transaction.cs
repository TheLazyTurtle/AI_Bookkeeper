using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DataManager.Services;

namespace DataManager.Models;

public enum CategorySelection
{
    HandFill,
    AutoFill,
    NotSet
}

public class Transaction: INotifyPropertyChanged
{
    [NotMapped] private CategorySelection _categorySelection = CategorySelection.NotSet;
    [NotMapped] private Category? _category;
    [NotMapped] private int? _categoryId;
    
    [Key]
    public int TransactionId { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Account { get; set; }
    public string Counterparty { get; set; }
    public TransactionType DebitCredit { get; set; }
    public float Amount { get; set; }
    public string TransactionType { get; set; }
    public string Notifications { get; set; }

    public CategorySelection CategorySelection
    {
        get => _categorySelection;
        set
        {
            _categorySelection = value;
            OnPropertyChanged();
        }
    }
    
    [NotMapped] public Category? Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged();
        } 
    }

    public int? CategoryId
    {
        get => _categoryId;
        set
        {
            _categoryId = value;
            OnPropertyChanged();
        }
    }

    [NotMapped]
    public IEnumerable<Category> PossibleCategories => CategoryManager.GetCategoriesByType(DebitCredit);
    
    public bool Equals(Transaction other)
    {
        return other.Date.Equals(Date) &&
               other.Name == Name &&
               other.Account == Account &&
               other.Counterparty == Counterparty &&
               other.DebitCredit == DebitCredit &&
               Math.Abs(other.Amount - Amount) < 0.001 &&
               other.TransactionType == TransactionType &&
               other.Notifications == Notifications;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    public void SetCategory(Category? category, CategorySelection categorySelection)
    {
        Category = category;
        CategoryId = category?.Id ?? null;
        CategorySelection = category == null ? CategorySelection.NotSet : categorySelection;
    }
}