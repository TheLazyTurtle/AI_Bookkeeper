using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataManager.Models;
using DataManager.Services;

namespace Visualization;

public partial class CategoryManagerScreen
{
    public CategoryManagerScreen()
    {
        InitializeComponent();
    }

    private async void IncomeDataGrid_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        if (e.EditAction != DataGridEditAction.Commit) 
            return;

        if (e.Row.Item is not Category item)
            return;
        
        item.Type = TransactionType.Income;
    }

    private async void ExpenseDataGrid_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        if (e.EditAction != DataGridEditAction.Commit) 
            return;

        if (e.Row.Item is not Category item) 
            return;
        
        item.Type = TransactionType.Expense;
    }

    private async void DataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command != ApplicationCommands.Delete) 
            return;
        
        if (sender is not DataGrid dataGrid)
            return;

        foreach (var item in dataGrid.SelectedItems)
        {
            if (item is Category category)
                await CategoryManager.DeleteCategory(category);
        }
    }

    private async void Save_OnClick(object sender, RoutedEventArgs e)
    {
        foreach (var income in CategoryManager.Incoming)
        {
            await CategoryManager.AddOrUpdate(income);
        }
        
        foreach (var expense in CategoryManager.Expense)
        {
            await CategoryManager.AddOrUpdate(expense);
        }
    }
}