using System.Collections.ObjectModel;
using System.Linq;
using DataManager.Models;
using DataManager.Services;

namespace Visualization;

public partial class ReportWindow
{
    private readonly int _year;
    public ObservableCollection<Report> Incoming { get; } = new();
    public ObservableCollection<Report> Expense { get; } = new();
    
    public float TotalIncome { get; private set; }
    public float TotalExpense { get; private set; }
    public string TotalResult => (TotalIncome - TotalExpense).ToString("n2");
    public string HeaderLabel => $"Result for {_year}";
    
    public ReportWindow(int year)
    {
        _year = year;
        
        var reports = ReportManager.GetCategoriesWithTotalAmount(year).ToList();
        
        var totalAmount = ReportManager.GetTotalAmountForTransactionType(year).ToList();
        TotalIncome = totalAmount.First(r => r.Type == TransactionType.Income).Amount;
        TotalExpense = totalAmount.First(r => r.Type == TransactionType.Expense).Amount;

        var incomeList = reports.Where(r => r.Type == TransactionType.Income);
        foreach (var income in incomeList)
        {
            Incoming.Add(income);
        }

        var expenseList = reports.Where(r => r.Type == TransactionType.Expense);
        foreach (var expense in expenseList)
        {
            Expense.Add(expense);
        }
        
        InitializeComponent();
    }
}