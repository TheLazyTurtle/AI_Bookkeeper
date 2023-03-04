using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataManager.Services;

namespace DataManager.Models;

public class Transaction
{
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
    public int? CategoryId { get; set; }
    [NotMapped]
    public Category? Category { get; set; }
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
}