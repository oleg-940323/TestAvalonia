using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TestAvalonia.Models;

namespace TestAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private ObservableCollection<Book> commonBooks = new() {
            new Book() { Name = "Война и мир", Author = "Лев Толстой", Genre = "Роман" },
            new Book() { Name = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", Genre = "Фэнтези" },
            new Book() { Name = "Убийство в Восточном экспрессе", Author = "Агата Кристи", Genre = "Детектив" },
            new Book() { Name = "Властелин колец", Author = "Дж. Р. Р. Толкин", Genre = "Фэнтези" },
            new Book() { Name = "Тихий Дон", Author = "Михаил Шолохов", Genre = "Роман" },
            new Book() { Name = "Преступление и наказание", Author = "Федор Достоевский", Genre = "Роман" },
            new Book() { Name = "Девушка с татуировкой дракона", Author = "Стиг Ларссон", Genre = "Детектив" },
            new Book() { Name = "Убить пересмешника", Author = "Харпер Ли", Genre = "Роман" },
            new Book() { Name = "Три товарища", Author = "Эрих Мария Ремарк", Genre = "Роман" },
            new Book() { Name = "Мастер и Маргарита", Author = "Михаил Булгаков", Genre = "Фэнтези" }

        };

        public ObservableCollection<Book> CommonBooks
        {
            get => commonBooks;

            set
            {
                commonBooks = value;
                OnPropertyChanged(nameof(CommonBooks));
            }
        }

        private ObservableCollection<Book> filtrCollect = new();

        private ObservableCollection<Group> sortBooks = new();
        private ObservableCollection<Group> SortBooks
        {
            get => sortBooks;

            set
            {
                sortBooks = value;
                OnPropertyChanged(nameof(SortBooks));
            }
        }

        /// <summary>
        /// Сортировка книг по жанрам
        /// </summary>
        /// <param name="books"> Коллекция с неотсортированными книгами </param>
        public void Sort(ObservableCollection<Book> book)
        {
            // Получаем словарь сортированных книг
            Dictionary<string, List<Book>> booksByGenre = book.GroupBy(b => b.Genre).ToDictionary(g => g.Key, g => g.ToList());

            SortBooks.Clear();

            // Получаем коллекцию с коллекциями сортированных книг 
            foreach (var key in booksByGenre)
                SortBooks.Add(new Group() { Genre = key.Key, Books = new ObservableCollection<Book>(key.Value) });

            OnPropertyChanged("SortBooks");
        }

        public void SearchBooks(string text)
        {
            Sort(new ObservableCollection<Book>(CommonBooks.Where(b => b.Name.StartsWith(text))));
        }
    }
}