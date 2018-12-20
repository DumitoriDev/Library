using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibraryClass;
using MahApps.Metro.Controls;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для DeleteGenre.xaml
    /// </summary>
    public partial class DeleteGenre : MetroWindow
    {
        private readonly GenreRepository _genreRepository = new GenreRepository();
        private readonly BookRepository _bookRepository = new BookRepository();
        public bool Status = false;
        public DeleteGenre()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TextBox.Text))
                {
                    MessageBox.Show("Данные пусты!", "Error");
                    return;
                }

                var tmp = this._genreRepository.Get(genre => genre.Name == this.TextBox.Text);
              
                   
                if (tmp is null)
                {
                    MessageBox.Show("Жанра нет!", "Error");
                    return;
                }

                if (this._bookRepository.Check(book => book.Genre.All(genre => genre.Name == this.TextBox.Text)))
                {
                    MessageBox.Show("Жанр привязан к книгам, удаление невозможно" , "Error");
                    return;
                }
                this._genreRepository.Delete(tmp.Id);
                this.Status = true;
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
          
        }
    }
}
