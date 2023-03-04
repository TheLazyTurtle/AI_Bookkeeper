using System.ComponentModel.DataAnnotations;

namespace DataManager.Models;

public enum TransactionType 
{
    Income,
    Expense
}

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public TransactionType Type { get; set; }
    public List<Transaction> Transactions { get; set; }
}