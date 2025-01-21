using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public IReadOnlyList<int> TrackedTransactionYears { get; } = TransactionManager.GetYears();
    public ObservableCollection<Transaction> Transactions { get; } = new();
    public Transaction SelectedTransaction { get; set; }
    public int SelectedTransactionYear { get; set; } = TransactionManager.GetLatestYear();
    
    public MainWindow()
    {
        InitializeComponent();
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

    private async void ComboBox_OnDropDownClosed(object sender, EventArgs eventArgs)
    {
        var category = (Category)((ComboBox)sender).SelectedItem;
        var transaction = (Transaction)((FrameworkElement)sender).DataContext;
        var actual = TransactionManager.Transactions.First(t => t.Equals(transaction));
        actual.SetCategory(category, CategorySelection.HandFill);
        await TransactionManager.UpdateTransaction(actual);
    }

    private void TrainModel_OnClick(object sender, RoutedEventArgs e)
    {
        PredictionManager.Retrain();
    }

    private async void PredictCategory_OnClick(object sender, RoutedEventArgs e)
    {
        var transaction = SelectedTransaction;
        var category = PredictionManager.Predict(transaction);
        
        var actualTransaction = TransactionManager.Transactions.First(t => t.Equals(transaction));
        actualTransaction.SetCategory(category, CategorySelection.AutoFill);
        await TransactionManager.UpdateTransaction(actualTransaction);
    }

    private async void PredictCategoryOnAllEmpty_OnClick(object sender, RoutedEventArgs e)
    {
        var transactions = TransactionManager.Transactions.Where(t => t.CategoryId == null);
        foreach (var transaction in transactions)
        {
            var category = PredictionManager.Predict(transaction);
            transaction.SetCategory(category, CategorySelection.AutoFill);
            await TransactionManager.UpdateTransaction(transaction);
        }
    }

    private void YearChanged(object sender, SelectionChangedEventArgs e)
    {
        Transactions.Clear();
        var transactions = TransactionManager.GetTransactionsOfYear(SelectedTransactionYear);
        foreach (var transaction in transactions)
        {
            Transactions.Add(transaction);
        }
    }

    private void OpenReportButton_OnClick(object sender, RoutedEventArgs e)
    {
        new ReportWindow(SelectedTransactionYear).Show();
    }
}