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
    /// Логика взаимодействия для DeleteEdition.xaml
    /// </summary>
    public partial class DeleteEdition : MetroWindow
    {

        private readonly EditionRepository _editionRepository = new EditionRepository();
        private readonly BookRepository _bookRepository = new BookRepository();
        public bool Status = false;
        public DeleteEdition()
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

                var tmp = this._editionRepository.Get(type => type.Name == this.TextBox.Text);

                if (tmp is null)
                {
                    MessageBox.Show("Издательство не удалось найти!", "Error");
                    return;
                }

                if (this._bookRepository.Check(book => book.Edition.Name == this.TextBox.Text))
                {
                    MessageBox.Show("Издательство привязано к книгам, удаление невозможно", "Error");
                    return;
                }

                this._editionRepository.Delete(tmp.Id);
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
