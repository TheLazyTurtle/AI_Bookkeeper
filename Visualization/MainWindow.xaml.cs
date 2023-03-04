using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DataManager.Models;
using DataManager.Services;
using Microsoft.Win32;

namespace Visualization;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private CategoryManagerScreen? _categoryManagerScreen;
    
    public MainWindow()
    {
        InitializeComponent();
        
        Transactions.ItemsSource = TransactionManager.Transactions;
    }

    private async void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            FileName = "Document",
            DefaultExt = ".csv",
            Filter = "CSV documents (.csv)|*.csv"
        };
        
        var result = dialog.ShowDialog();

        if (result != true) 
            return;
        
        var filename = dialog.FileName;
        var transactions = CsvParser.Parse(filename);
        await TransactionManager.AddTransactions(transactions);
    }

    private void OpenCategoryButton_OnClick(object sender, RoutedEventArgs e)
    {
        _categoryManagerScreen = new CategoryManagerScreen();
        _categoryManagerScreen.Show();
    }

    private async void Selector_OnSelected(object sender, RoutedEventArgs args)
    {
        var category = (Category)((ComboBox)sender).SelectedItem;
        var transaction = (Transaction)((FrameworkElement)sender).DataContext;
        var actual = TransactionManager.Transactions.First(t => t.Equals(transaction));
        actual.Category = category;
        await TransactionManager.UpdateTransaction(actual);
    }
}