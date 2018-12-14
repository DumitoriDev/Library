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
    /// Логика взаимодействия для AddNewGenre.xaml
    /// </summary>
    public partial class AddNewGenre : MetroWindow
    {
        private readonly GenreRepository _genreRepository = new GenreRepository();

        public AddNewGenre()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
           
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TextBox.Text))
                {
                    MessageBox.Show("Вы ввели пустые данные!", "Error");
                    return;
                }

                if (_genreRepository.Check(genre => genre.Name != this.TextBox.Text))
                {
                    var tmp = new Genre { Name = this.TextBox.Text };
                    this._genreRepository.Add(tmp);
                }
                
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
            
        }

       
    }
}
