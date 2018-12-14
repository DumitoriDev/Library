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
using LibraryClass.source;
using MahApps.Metro.Controls;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для AddAuthor.xaml
    /// </summary>
    public partial class AddAuthor : MetroWindow
    {
        private readonly ICollection<Author> _authors = null;

        public AddAuthor(ICollection<Author> authors)
        {
            try
            {
               
                InitializeComponent();
                this._authors = authors;
                ComboBox.ItemsSource = new AuthorRepository().GetAll();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
           
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var res = (sender as ComboBox)?.SelectedItem;
                if (_authors.All(author => res != null && author.Id != ((Author) res).Id))
                {
                    _authors.Add((Author)res);
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
