using System.Collections.ObjectModel;
using System.Linq;
using DataManager.Models;
using DataManager.Services;

namespace Visualization;

public partial class ReportWindow
{
    public ObservableCollection<Report> Reports { get; set; }
    public ReportWindow()
    {
        InitializeComponent();
        Reports = new ObservableCollection<Report>(ReportManager.GetCategoriesWithTotalAmount());
        var totalAmount = ReportManager.GetTotalAmountForTransactionType().ToList();
        Reports.Add(totalAmount.First(r => r.Type == TransactionType.Income));
        Reports.Add(totalAmount.First(r => r.Type == TransactionType.Expense));
        
        ReportDataGridIncoming.ItemsSource = Reports.Where(r => r.Type == TransactionType.Income);
        ReportDataGridExpense.ItemsSource = Reports.Where(r => r.Type == TransactionType.Expense);
    }
}