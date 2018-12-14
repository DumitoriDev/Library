using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library;
using LibraryClass;
using LibraryClass.source;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;
using Type = LibraryClass.Type;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : MetroWindow
    {
        private readonly BookRepository _bookHelper = new BookRepository();
        private readonly ReaderRepository _readerRepository = new ReaderRepository();
        private readonly GenreRepository _genreRepository = new GenreRepository();
        private readonly AuthorRepository _authorRepository = new AuthorRepository();
        private readonly TypeRepository _typeRepository = new TypeRepository();
        private readonly BookAndReaderRepository _bookAndReaderRepository = new BookAndReaderRepository();



        private int _fromBook = 0;
        private int _beforeBook = 10;


        public void Setting()
        {

            this.GridBooks.ItemsSource = _bookHelper.GetRange(_fromBook, _beforeBook);
            this.GridReaders.ItemsSource = _readerRepository.GetAll();

            this.DataContext = this;


        }

        private void GridBooks_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {

            e.NewItem = new Book();
            ((Book)e.NewItem).Type = new LibraryClass.Type();
            ((Book)e.NewItem).Author = new List<Author>();
            ((Book)e.NewItem).Edition = new Edition();
            ((Book)e.NewItem).Genre = new List<Genre>();
            ((Book)e.NewItem).Date = DateTime.Now.Year;
            ((Book)e.NewItem).Language = new Language();
            ((Book)e.NewItem).Languages = new LanguageRepository().GetAll();
            ((Book)e.NewItem).Types = new TypeRepository().GetAll();
            ((Book)e.NewItem).Editions = new EditionRepository().GetAll();


        }

        public MainWindow()
        {

            InitializeComponent();
            Setting();

        }

        private bool SaveBook()
        {

            foreach (var tmpBook in (List<Book>)this.GridBooks.ItemsSource)
            {

                if (BookRepository.Check(tmpBook))
                    continue;

                MessageBox.Show("Не все поля заполнены! Сохранение невозможно!");
                return false;

            }

            this._bookHelper.UpdateAll((List<Book>)this.GridBooks.ItemsSource);
            return true;

        }

        private bool SaveReader()
        {

            foreach (var tmpBook in (List<Reader>)this.GridReaders.ItemsSource)
            {

                if (ReaderRepository.Check(tmpBook))
                    continue;

                MessageBox.Show("Не все поля заполнены! Сохранение невозможно!");
                return false;

            }

            this._readerRepository.UpdateAll((List<Reader>)this.GridReaders.ItemsSource);
            return true;

        }

        private void MenuItem_OnClick_Save_All(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.SaveBook() && this.SaveReader())
                {
                    MessageBox.Show("Сохранение завершено!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }




        }

        private void MenuItem_OnClick_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.SaveBook())
                {
                    MessageBox.Show("Сохранение завершено!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }




        }

        private void MenuItem_OnClick_Save_Reader(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.SaveReader())
                {
                    MessageBox.Show("Сохранение завершено!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void MenuItem_OnClick_Add_Image(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((GridBooks.CurrentItem as Book) is null)
                    return;

                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.JPG;*.JPEG)|*.png;*.jpeg;*.jpg";

                if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                var file = new FileInfo(openFileDialog.FileName);

                if (file.Length > 1_000_000)
                {
                    MessageBox.Show("Файл слишком большой!");
                    return;
                }

                var bytes = ImageHelper.ImageToBytes(openFileDialog.FileName);

                ((Book)GridBooks.CurrentItem).Cover = bytes;
                ((Book)GridBooks.CurrentItem).Img.Source = ImageHelper.BytesToImage(bytes);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void MenuItem_OnClick_Info(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    this.GridBooks.IsEnabled = false;
                }
                catch
                {
                    MessageBox.Show("Пожалуйста, закончите создавать книгу, а потом перейдите в подробную информацию. Для завершения создания книги нужно выделить другую книгу, либо создать новую.");
                    this.GridBooks.IsEnabled = true;
                    return;
                }

                if ((GridBooks.SelectedItem as Book) != null)
                {

                    this.CoverGrid.Source = ((Book)GridBooks.SelectedItem)?.Img.Source;
                    this.GridAuthor.ItemsSource = ((Book)GridBooks.SelectedItem)?.Author.ToList();
                    this.GridGenre.ItemsSource = ((Book)GridBooks.SelectedItem)?.Genre.ToList();
                    this.DescBook.Text = ((Book)GridBooks.SelectedItem)?.Desc;
                    this.FlyoutInfo.IsOpen = true;
                }

                this.GridBooks.IsEnabled = true;



            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void FlyoutInfo_OnClosingFinished(object sender, RoutedEventArgs e)
        {
            try
            {

                ((Book)GridBooks.SelectedItem).Author = this.GridAuthor.ItemsSource as ICollection<Author>;
                ((Book)GridBooks.SelectedItem).Genre = this.GridGenre.ItemsSource as ICollection<Genre>;
                ((Book)GridBooks.SelectedItem).Desc = this.DescBook.Text;

                this.GridBooks.IsEnabled = true;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void MenuItem_OnClick_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridBooks.SelectedIndex == -1 || (this.GridBooks.SelectedItem as Book) is null)
                    return;

                try
                {
                    this.GridBooks.IsEnabled = false;
                }
                catch
                {
                    MessageBox.Show("Пожалуйста, закончите создавать книгу, а потом пытайтесь её удалить. Для завершения создания книги нужно выделить другую книгу, либо создать новую.");
                    this.GridBooks.IsEnabled = true;
                    return;
                }


                var res = this.GridBooks.ItemsSource as List<Book>;

                if (this._bookHelper.Get(((Book)GridBooks.SelectedItem).Id) is null)
                {
                    res?.Remove(((Book)GridBooks.SelectedItem));
                }

                else
                {
                    this._bookHelper.Delete(((Book)GridBooks.SelectedItem).Id);
                    res = this._bookHelper.GetRange(this._fromBook, this._beforeBook);
                }

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = res;
                this.GridBooks.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");

            }

        }

        private void GridBooks_OnCurrentCellChanged(object sender, EventArgs e)
        {
            ;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(GridBooks.CurrentItem is Book book && !(((Book)this.GridBooks.SelectedItem) is null))) return;


            switch ((sender as ComboBox)?.SelectedItem)
            {
                case Type type:
                    ((Book)this.GridBooks.SelectedItem).Type = type;
                    break;

                case Edition edition:
                    ((Book)this.GridBooks.SelectedItem).Edition = edition;
                    break;

                case Language language:
                    ((Book)this.GridBooks.SelectedItem).Language = language;
                    break;

                default:
                    return;
            }



        }

        private void MenuItem_OnClick_Add_Author(object sender, RoutedEventArgs e)
        {
            try
            {
                this.GridAuthor.ItemsSource = null;
                var tmp = new AddAuthor((GridBooks.SelectedItem as Book)?.Author);
                tmp.ShowDialog();
                this.GridAuthor.ItemsSource = (GridBooks.SelectedItem as Book)?.Author.ToList();
                ;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void MenuItem_OnClick_Delete_Author(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridAuthor.SelectedIndex == -1)
                    return;

                var res = (this.GridAuthor.ItemsSource as List<Author>);
                res?.Remove(this.GridAuthor.SelectedItem as Author);
                this.GridAuthor.ItemsSource = null;
                this.GridAuthor.ItemsSource = res;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void MenuItem_OnClick_Add_Genre(object sender, RoutedEventArgs e)
        {
            try
            {
                this.GridGenre.ItemsSource = null;
                var tmp = new AddGenre((GridBooks.SelectedItem as Book)?.Genre);
                tmp.ShowDialog();
                this.GridGenre.ItemsSource = (GridBooks.SelectedItem as Book)?.Genre.ToList();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void MenuItem_OnClick_Delete_Genre(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridGenre.SelectedIndex == -1)
                    return;

                var res = (this.GridGenre.ItemsSource as List<Genre>);
                res?.Remove(this.GridGenre.SelectedItem as Genre);
                this.GridGenre.ItemsSource = null;
                this.GridGenre.ItemsSource = res;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void ArrowRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _fromBook += 10;
                _beforeBook += 10;

                var books = _bookHelper.GetRange(this._fromBook, this._beforeBook);

                if (books.Count < 1)
                {
                    MessageBox.Show("Последняя страница!", "Info");
                    _fromBook -= 10;
                    _beforeBook -= 10;
                    return;
                }

                if (!this.SaveBook())
                    return;

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = _bookHelper.GetRange(this._fromBook, this._beforeBook);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void ArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((this._fromBook - 10) < 0)
                {
                    MessageBox.Show("Последняя страница!", "Info");
                    return;
                }

                if (!this.SaveBook())
                    return;


                this._fromBook -= 10;
                this._beforeBook -= 10;


                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = _bookHelper.GetRange(this._fromBook, this._beforeBook);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void MenuItem_OnClick_Add_New_Genre(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewGenre();
                tmp.ShowDialog();



            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void MenuItem_OnClick_Add_New_Author(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewAuthor();
                tmp.ShowDialog();


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void FlyoutInfo_OnClosingFinished_Reader(object sender, RoutedEventArgs e)
        {
            try
            {

                ((Reader)GridReaders.SelectedItem).Password = ((this.GridInfoLogin.ItemsSource as List<Reader>) ?? throw new InvalidOperationException()).First().Password;
                ((Reader)GridReaders.SelectedItem).BookAndReaders = (((ICollection<BookAndReader>)this.GridInfoBooks.ItemsSource) ?? throw new InvalidOperationException());

                this.GridReaders.IsEnabled = true;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void MenuItem_OnClick_Info_Reader(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    this.GridReaders.IsEnabled = false;
                }
                catch
                {
                    MessageBox.Show("Пожалуйста, закончите создавать пользователя, а потом перейдите в подробную информацию. Для завершения создания пользователя нужно выделить другого, либо начать создавать нового.");
                    this.GridReaders.IsEnabled = true;
                    return;
                }

                if ((this.GridReaders.SelectedItem as Reader) != null)
                {
                    var tmpList = new List<Reader> { ((Reader)this.GridReaders.SelectedItem) };
                    this.GridInfoLogin.ItemsSource = tmpList;
                    this.GridInfoBooks.ItemsSource = tmpList.First().BookAndReaders;

                    this.FlyoutInfoReader.IsOpen = true;
                }

                this.GridReaders.IsEnabled = false;



            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void MenuItem_OnClick_Add_Book(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    if (((this.GridInfoLogin.ItemsSource as List<Reader>) ?? throw new InvalidOperationException()).First() is null) return;
                    var tmp = new AddBook(((List<Reader>)this.GridInfoLogin.ItemsSource).First());
                    tmp.ShowDialog();


                    if (((Reader)this.GridReaders.SelectedItem) == null) return;

                    var tmpList = new List<Reader> { ((Reader)this.GridReaders.SelectedItem) };
                    this.GridInfoBooks.ItemsSource = null;
                    this.GridInfoBooks.ItemsSource = tmpList.First().BookAndReaders;
                    this.GridBooks.ItemsSource = null;
                    this.GridBooks.ItemsSource = _bookHelper.GetRange(this._fromBook, this._beforeBook);

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void  MenuItem_OnClick_Take_Book(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    var reader = this.GridInfoBooks.SelectedItem as BookAndReader;
                    if (reader is null)
                    {
                        return;
                    }

                    var resPrice = ((DateTime.Now - reader.DateEnd.StartTime).Days) * reader.Book.Price;

                    await this.ShowMessageAsync("Info", $"Человек должен заплатить - {resPrice}");

                    reader.Book.Count++;
                    this._bookAndReaderRepository.Delete(reader.Id);
                    

                    this.GridBooks.ItemsSource = null;
                    this.GridBooks.ItemsSource = _bookHelper.GetRange(this._fromBook, this._beforeBook);

                    if ((this.GridReaders.SelectedItem as Reader) == null) return;

                    this.GridInfoBooks.ItemsSource = null;
                    this.GridInfoBooks.ItemsSource = ((Reader) this.GridReaders.SelectedItem).BookAndReaders;

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void GridReaders_OnAddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new Reader();
            ((Reader)e.NewItem ).BookAndReaders = new List<BookAndReader>();
        }
    }
}
