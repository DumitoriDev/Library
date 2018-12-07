using System;
 using System.Collections.Generic;
using System.ComponentModel;
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

        

        private  void GridBooks_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
           
        
            if (((GridBooks.SelectedCells) is null))
            {
                ;
            }

           
        }

        private readonly BookRepository _bookHelper = new BookRepository();
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }
        public List<Genre> Genres { get; set; }
       

        public void Setting()
        {
            this.Books = _bookHelper.GetAllImg();
           
            //Image img = new Image { Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("1.jpg"))) };
            //var bytes = ImageHelper.ImageToBytes(img.Source as BitmapImage);
            //var first = this.Books.First();
            //first.Cover = bytes;
            //_bookHelper.Update(first);
            //this.GridBooks.CurrentCellChanged += GridBooks_CurrentCellChanged;

            

        }

        private void GridBooks_CurrentCellChanged(object sender, EventArgs e)
        {

            //if ((GridBooks.CurrentItem as Book) is null)
            //{
            //    this.GridAuthor.ItemsSource = null;
            //    return;
            //}
            //this.Authors = (this.GridBooks.CurrentItem as Book).AuthorId.ToList();
            //GridAuthor.ItemsSource = this.Authors;
            
        }

        private void GridBooks_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
           
        }

        public MainWindow()
        {
            
         
           
            InitializeComponent();
        
            this.FlyoutInfo.Theme = FlyoutTheme.Dark;
            this.DataContext = this;
            Setting();
            
        }

        private void GridBooks_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_OnClick_Info(object sender, RoutedEventArgs e)
        {

            if ((GridBooks.CurrentItem as Book) is null)
                        return;

            this.Authors = (this.GridBooks.CurrentItem as Book).AuthorId.ToList();
            this.Genres = (this.GridBooks.CurrentItem as Book).GenreId.ToList();
            GridAuthor.ItemsSource = this.Authors;
            GridGenre.ItemsSource = Genres;
            DescBook.Text = (this.GridBooks.CurrentItem as Book).Desc;
            FlyoutInfo.IsOpen = true;
           

        }

        private void FlyoutInfo_OnClosingFinished(object sender, RoutedEventArgs e)
        {
            if ((GridBooks.SelectedItem) is null)
                return;
           ( GridBooks.SelectedItem as Book).Desc = this.DescBook.Text;

        }
    }
}
