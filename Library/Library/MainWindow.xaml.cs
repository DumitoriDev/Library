using System;
 using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library;
using LibraryClass;
using MahApps.Metro.Controls;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
   

    public partial class MainWindow : MetroWindow
    {
      
        private readonly BookRepository _bookHelper = new BookRepository();
        public List<Book> Books { get; set; }
      
        public MainWindow()
        {
            
            this.Books = _bookHelper.GetAll().ToList();
            this.Books[0].Img.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("1.jpeg")));
            this.DataContext = this;
            InitializeComponent();
        }

        private void GridBooks_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ;
        }
    }
}
