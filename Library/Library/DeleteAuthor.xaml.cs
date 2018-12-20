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
    /// Логика взаимодействия для DeleteAuthor.xaml
    /// </summary>
    public partial class DeleteAuthor : MetroWindow
    {

        private readonly AuthorRepository _authorRepository = new AuthorRepository();
        private readonly BookRepository _bookRepository = new BookRepository();
        public bool Status = false;
        public DeleteAuthor()
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

                var tmp = this._authorRepository.Get(author => author.Name == this.TextBox.Text);

                if (tmp is null)
                {
                    MessageBox.Show("Автора не удалось найти!", "Error");
                    return;
                }

                if (this._bookRepository.Check(book => book.Author.All(author => author.Name == this.TextBox.Text)))
                {
                    MessageBox.Show("Автор привязан к книгам, удаление невозможно", "Error");
                    return;
                }

                this._authorRepository.Delete(tmp.Id);
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
