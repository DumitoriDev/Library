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
    /// Логика взаимодействия для AddGenre.xaml
    /// </summary>
    public partial class AddGenre : MetroWindow
    {

        private readonly ICollection<Genre> _genres = null;

        public AddGenre(ICollection<Genre> genres)
        {

            InitializeComponent();
            this._genres = genres;
            ComboBox.ItemsSource = new GenreRepository().GetAll();

        }

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                var res = (sender as ComboBox)?.SelectedItem;
                if (_genres.All(author => res != null && author.Id != ((Genre)res).Id))
                {
                    _genres.Add((Genre)res);
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
