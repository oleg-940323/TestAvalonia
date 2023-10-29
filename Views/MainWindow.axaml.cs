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

            // ���������� �������
            Search.TextChanged += context.onTextChanged;
            //View1.RowDragStarted += context.RowDragStarted;
            //View1.RowDrop += context.OnTreeViewElementDroped;
            View1.SelectionMode = SelectionMode.Multiple;
            View1.Classes = "NodesDragAndDrop";
            dataGrid.PointerReleased += context.OnPointerReleased;
            dataGrid.RowDrop += context.OnTreeViewElementDroped;
            dataGrid.KeyUp += OnDeleteBook;

        }

        // �������� ���������� ��������� � �������
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