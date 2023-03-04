using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DataManager.Models;
using DataManager.Services;

namespace Visualization;

public partial class CategoryManagerScreen
{
    public CategoryManagerScreen()
    {
        InitializeComponent();
        
        Categories.CellEditEnding += OnCellEdit;
        Categories.DataContext = CategoryManager.Categories;
        Categories.ItemsSource = CategoryManager.Categories;
    }

    private async void OnCellEdit(object? sender, DataGridCellEditEndingEventArgs args)
    {
        var category = (Category)args.Row.Item;
        
        if (args.EditingElement is TextBox t)
        {
            var text = t.Text;
            category.Name = text;
        }
        else if (args.EditingElement is ComboBox c)
        {
            var index = c.SelectedIndex;
            category.Type = (TransactionType)index;
        }
        
        await CategoryManager.AddOrUpdate(category);
    }

    private async void OnClick_RemoveSelected(object sender, RoutedEventArgs e)
    {
        var category = (Category)Categories.SelectedItem;
        await CategoryManager.DeleteCategory(category);
    }
}