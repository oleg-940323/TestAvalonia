using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestAvalonia.Models
{
    public class Book : INotifyPropertyChanged
    {
        #region
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        // Жанр
        public string? Genre { get; set; }

        // Название книги
        public string? Name { get; set; }

        // Автор
        public string? Author { get; set; }

        // Подуровень
        private ObservableCollection<Book>? children = new();
        public ObservableCollection<Book>? Children { get => children; set { children = value; OnPropertyChanged("Name"); } }

        // Раскрыт
        private bool isExpanded = true;
        public bool IsExpanded { get => isExpanded; set { isExpanded = value; OnPropertyChanged("Name"); } }

        // Есть еще подуровень
        private bool hasClild = false;
        public bool HasClild { get => hasClild; set {hasClild = value; OnPropertyChanged("Name"); } }

        // Индекс книги
        public UInt16 index;
    }
}