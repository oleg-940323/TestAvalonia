using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TestAvalonia.Models;
using TestAvalonia.ViewModels;

namespace TestAvalonia.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel context = new();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = context;

            // Присвоение событий
            Search.TextChanged += context.OnTextChanged;
            treeView.RowDragStarted += context.RowDragStarted;
            dataGrid.PointerReleased += context.OnPointerReleased;
            dataGrid.RowDrop += context.OnRowDroped;
            dataGrid.KeyUp += OnDeleteBook;
        }

        // Удаление выделенных элементов в таблице
        void OnDeleteBook(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var cnt = dataGrid.RowSelection.SelectedItems.Count;
                ObservableCollection<Book> items = new((IReadOnlyList<Book>)dataGrid.RowSelection.SelectedItems);

                for (var i = 0; i < cnt; i++)
                    context.filtrCollect.Remove(items[i]);
            }
        }
    }
}