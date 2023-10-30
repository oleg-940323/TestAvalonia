using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
using Avalonia.Controls;
using System.Timers;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DynamicData.Binding;
using Microsoft.VisualBasic;
using TestAvalonia.Models;

namespace TestAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        // Книги
        private readonly ObservableCollection<Book> commonBooks = new() {
            new Book() { Name = "Война и мир", Author = "Лев Толстой", Genre = "Роман", index = 0},
            new Book() { Name = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", Genre = "Фэнтези", index = 1},
            new Book() { Name = "Убийство в Восточном экспрессе", Author = "Агата Кристи", Genre = "Детектив", index = 2},
            new Book() { Name = "Властелин колец", Author = "Дж. Р. Р. Толкин", Genre = "Фэнтези", index = 3},
            new Book() { Name = "Тихий Дон", Author = "Михаил Шолохов", Genre = "Роман", index = 4},
            new Book() { Name = "Преступление и наказание", Author = "Федор Достоевский", Genre = "Роман", index = 5},
            new Book() { Name = "Девушка с татуировкой дракона", Author = "Стиг Ларссон", Genre = "Детектив", index = 6},
            new Book() { Name = "Убить пересмешника", Author = "Харпер Ли", Genre = "Роман", index = 7},
            new Book() { Name = "Три товарища", Author = "Эрих Мария Ремарк", Genre = "Роман", index = 8},
            new Book() { Name = "Мастер и Маргарита", Author = "Михаил Булгаков", Genre = "Фэнтези", index = 9}

        };

        // Книги, помещенные в таблицу
        public ObservableCollection<Book> filtrCollect = new();
        public FlatTreeDataGridSource<Book> FiltrCollect { get; set; }

        // Перетаскиваемые книги
        private ObservableCollection<Book> temprCollect = new();

        // Книги, отображенные в дереве
        private ObservableCollection<Book> sortBooks = new();
        public HierarchicalTreeDataGridSource<Book> SortBooks { get; set; }
        

        public MainWindowViewModel()
        {
            Sort(commonBooks);

            // Инициализация дерева
             SortBooks = new HierarchicalTreeDataGridSource<Book>(sortBooks)
             {
                 Columns =
                 {
                     //new HierarchicalExpanderColumn<Book>( new TextColumn<Book, string>("", x => x.Name), x => x.Children, x => x.HasClild, x => x.IsExpanded)
                     new HierarchicalExpanderColumn<Book>( new TextColumn<Book, string>("", x => x.Name), x => x.Children)
                 }
             };

            // Инициализация таблицы
            FiltrCollect = new FlatTreeDataGridSource<Book>(filtrCollect)
            {
                Columns =
                {
                    new TextColumn<Book, string>( "Жанр", x => x.Genre),
                    new TextColumn<Book, string>("Название", x => x.Name),
                    new TextColumn<Book, string>("Автор", x => x.Author)
                }
            };

            // Включение множественного выбора
            SortBooks.RowSelection!.SingleSelect = false;
            FiltrCollect.RowSelection!.SingleSelect = false;
        }

        // Сортировка книг по жанрам
        public void Sort(ObservableCollection<Book> book)
        {
            // Получаем словарь сортированных книг
            Dictionary<string, List<Book>> booksByGenre = book.GroupBy(b => b.Genre).ToDictionary(g => g.Key, g => g.ToList());

            /*foreach (var elem in commonBooks)
            {
                elem.HasClild = false;
                elem.IsExpanded = true;
            }*/
            sortBooks.Clear();

            // Получаем коллекцию с коллекциями сортированных книг 
            foreach (var key in booksByGenre)
                //sortBooks.Add(new Book() { Name = key.Key, Children = new ObservableCollection<Book>(key.Value), HasClild = true });
                sortBooks.Add(new Book() { Name = key.Key, Children = new ObservableCollection<Book>(key.Value)});
        }

        // Сортировка дерева по изменению текста
        public void OnTextChanged(object sender, RoutedEventArgs e)
        {
            Sort(new ObservableCollection<Book>(commonBooks.Where(b => b.Name.ToUpper().StartsWith((sender as TextBox).Text.ToUpper()))));
        }

        // Начало переноса выделенных элементов
        public void RowDragStarted(object? sender, TreeDataGridRowDragStartedEventArgs e)
        {
            temprCollect.Clear();

            foreach (Book i in e.Models)
                AddInCollect(i);
        }

        // Добавление выделенных элементов (рекурсия) 
        void AddInCollect(Book book)
        {
            if (book.Children.Count != 0)
                foreach (var childBook in book.Children)
                    AddInCollect(childBook);
            else
                temprCollect.Add(book);
        }

        // Задание параметров брошенному элементу в дереве
        public void OnTreeViewElementDroped(object? sender, TreeDataGridRowDragEventArgs e)
        {
            var book = (e.TargetRow.DataContext as Book);

            if (e.Position == TreeDataGridRowDropPosition.Inside)
            {
                book.HasClild = true;
                book.IsExpanded = true;
            }
            else if (e.Position != TreeDataGridRowDropPosition.None)
            {
                foreach (var tempr in sortBooks)
                {
                    if (tempr.Children.Count > 0)
                        SearchBook(book);
                    else
                    {
                        tempr.IsExpanded = false;
                        tempr.HasClild = false;
                    }
                }
            }
        }

        void SearchBook(Book books)
        {
            foreach (var book in books.Children)
            {
                if (book.Children.Count > 0)
                    SearchBook(book);
                else
                {
                    book.IsExpanded = false;
                    book.HasClild = false;
                }
            }
        }

        public bool OnSubTreeViewElementDroped(ObservableCollection<Book> children, UInt16 index, bool act)
        {
            foreach (var item in children)
            {
                if (item.Children != null && item.Children.Count > 0)
                {
                    if (OnSubTreeViewElementDroped(item.Children, index, act))
                        return true;
                }
                else if (item.index == index)
                {
                    if (act)
                    {
                        item.IsExpanded = true;
                        item.HasClild = true;
                    }
                    else
                    {
                        item.IsExpanded = false;
                        item.HasClild = false;
                    }
                    return true;
                }

            }
            return false;
        }

        public void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            foreach (var item in temprCollect)
                if (!filtrCollect.Contains(item))
                    filtrCollect.Add(item);

            temprCollect.Clear();
        }
    }
}