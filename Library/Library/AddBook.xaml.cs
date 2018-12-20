using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using MahApps.Metro.Controls.Dialogs;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : MetroWindow
    {
        private readonly BookRepository _bookRepository = new BookRepository();
        private readonly BookAndReaderRepository _readerRepository = new BookAndReaderRepository();
        private readonly Reader _reader;
        public AddBook(Reader tmpreader)
        {
            InitializeComponent();
            this._reader = tmpreader;

        }



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ComboBoxAuthor.Text) || string.IsNullOrEmpty(ComboBoxName.Text))
                {
                    MessageBox.Show("Вы ввели пустые данные!", "Error");
                    return;
                }


                var bookTmp = _bookRepository.Get(book =>
                    book.Author.Any(author => author.Name == ComboBoxAuthor.Text));

                if (bookTmp is null)
                {
                    MessageBox.Show("Произведение не найдено!", "Error");
                    return;
                }

                if (bookTmp.Count > 0)
                {
                    bookTmp.Count--;
                    this._bookRepository.Update(bookTmp);

                    var readerTmp =
                      _readerRepository.Get(reader =>
                          reader.Book.Equals(bookTmp) && reader.DateEnd.EndTime ==
                          DateTime.Now.AddDays(Convert.ToInt16(this.Days.Value)) && reader.DateEnd.StartTime == DateTime.Now) ?? new BookAndReader
                          {
                              Book = bookTmp,
                              DateEnd = new UntilDate { EndTime = DateTime.Now.AddDays(Convert.ToInt16(this.Days.Value)), StartTime = DateTime.Now }
                          };

                    this._readerRepository.Add(readerTmp);
                    this._reader.BookAndReaders.Add(readerTmp);

                    
                }

                else
                {
                    MessageBox.Show("Количество данного произведения равно 0!", "Error");
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
