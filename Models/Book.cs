using Avalonia.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestAvalonia.Models
{
    // Класс книги
    public class Book : INotifyPropertyChanged
    {
        #region Реализация интерфейса 
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        // Жанр
        public string? Genre { get; set; }

        // Название книги
        private string? name;
        public string? Name { get => name; set { name = value; OnPropertyChanged(nameof(Name));} }

        // Автор
        public string? Author { get; set; }

        // Подуровень
        private ObservableCollection<Book>? children = new();
        public ObservableCollection<Book>? Children { get => children; set { children = value; OnPropertyChanged(nameof(Name)); } }

        // Раскрыт ли
        private bool isExpanded = true;
        public bool IsExpanded { get => isExpanded; set { isExpanded = value; OnPropertyChanged(nameof(Name)); } }

        // Индекс книги
        public UInt16 index;
    }
}