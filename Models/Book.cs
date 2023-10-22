using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.TextFormatting.Unicode;

namespace TestAvalonia.Models
{
    public class Book
    {
        // Жанр
        public string Genre { get; set; } = "Genre";

        // Название книги
        public string Name { get; set; } = "Name";

        // Автор
        public string Author { get; set; } = "Author";
    }
}