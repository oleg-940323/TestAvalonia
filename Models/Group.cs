using Avalonia.Data;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAvalonia.Models;
using System.Runtime.CompilerServices;

namespace TestAvalonia.Models
{
    public class Group
    {
        public string Genre { get; set; } = "Genre";

        // Коллекция книг с одним жанром
        public ObservableCollection<Book> Books { get; set; } = new();
    }
}