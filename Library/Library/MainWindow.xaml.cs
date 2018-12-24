using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using LibraryClass;
using LibraryClass.source;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MasaSam.Controls;
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
        private  BookRepository _bookHelper = new BookRepository();
        private  ReaderRepository _readerRepository = new ReaderRepository();
        private  BookAndReaderRepository _bookAndReaderRepository = new BookAndReaderRepository();

        public string[] Choose { get; set; } = { "Одиночный поиск", "Множественный" };

        private int _fromBook;
        private const int BeforeBook = 10;


        private int _fromReader = 0;
        private const int BeforeReader = 10;


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Update();
            
        }


        #region Update

        public async void Update() //Обновление всего
        {
            
            await Task.Factory.StartNew(() =>
            {
                try
                { 
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ProgressRing.IsActive = true;
                        this.Grid.IsEnabled = false;
                    });

                    DataBaseContext.Close();
                    DataBaseContext.Connect();

                    this._bookHelper = new BookRepository();
                    this._readerRepository = new ReaderRepository();
                    this._bookAndReaderRepository = new BookAndReaderRepository();
                    this.UpdateBooks(); 

                    this.UpdateReaders();
                    this.UpdateDebtor();

                    this.Dispatcher.BeginInvoke(new Action(()=>
                    {
                        this.UpdateInfoAll();
                        this.ShowMessageAsync("Информация", "Успешная загрузка");

                    }));
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ProgressRing.IsActive = false;
                        this.Grid.IsEnabled = true;
                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    
                }
            },TaskCreationOptions.LongRunning);
        
        }

        private void UpdateBooks()
        {
            var list =  this._bookHelper.GetRange(this._fromBook, BeforeBook);
           
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var book in list)
                {
                    book.Img = new Image {Source = ImageHelper.BytesToImage(book.Cover)};
                }
                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = list;

                
            }), DispatcherPriority.Normal);
        }

        private void UpdateReaders()
        {
            var list = _readerRepository.GetRange(this._fromReader, BeforeReader);
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
               this.GridReaders.ItemsSource = null;
               this.GridReaders.ItemsSource = list;
                
            }), DispatcherPriority.Normal);
        }

        private void UpdateDebtor()
        {
            var readers = _readerRepository.GetAll(reader =>
                reader.BookAndReaders.Any(andReader => andReader.DateEnd.EndTime < DateTime.Now));

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.GridInfoDebtor.ItemsSource = null;

                if (readers.Count < 1)
                {
                    this.GridReadersDebtor.ItemsSource = readers;
                    this.GridReadersDebtor.IsEnabled = false;
                    this.TextBlockDebtor.Visibility = Visibility.Visible;
                }
                else
                {
                    foreach (var reader in readers)
                    {
                        reader.BookAndReaders = reader.BookAndReaders.Where(dates => dates.DateEnd.EndTime < DateTime.Now).ToList();
                    }

                    this.GridReadersDebtor.ItemsSource = readers;
                    this.GridReadersDebtor.IsEnabled = true;
                    this.TextBlockDebtor.Visibility = Visibility.Collapsed;
                }
            }));
            
        }

        private void UpdateInfoAll()
        {

            this.TextBlockAllBooks.Text = $"Общее количество книжек: {this._bookHelper.GetSize()}";
            this.TextBlockAllReader.Text = $"Общее количество клиентов: {this._readerRepository.GetSize()}";
            this.TextBlockAllAuthor.Text = $"Общее количество записанных авторов: {new AuthorRepository().GetSize()}";
            this.TextBlockAllType.Text = $"Общее количество записанных типов: {new TypeRepository().GetSize()}";
            this.TextBlockAllGenre.Text = $"Общее количество записанных жанров: {new GenreRepository().GetSize()}";
            this.TextBlockDate.Text = $"Дата: {DateTime.Now.Date.ToShortDateString()}";


        }

        #endregion

        #region Save

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

        #endregion

        #region Click

        private void MenuItem_OnClick_Save_All(object sender, RoutedEventArgs e)
        {
            
            Task.Factory.StartNew(delegate
            {
                try
                {
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = true; });
                    if (this.SaveBook() && this.SaveReader())
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ShowMessageAsync("Информация", "Сохранение прошло успешно!");
                        });
                    }
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = false; });
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                }

             

            });


        }

        private void MenuItem_OnClick_Save(object sender, RoutedEventArgs e)
        {
           
            Task.Factory.StartNew(delegate
            {

                try
                {
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = true; });
                    if (this.SaveBook())
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ShowMessageAsync("Информация", "Сохранение прошло успешно!");
                        });

                    }
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = false; });
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                }

                

            });


        }

        private async void MenuItem_OnClick_Save_Reader(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = true; });
                    if (this.SaveReader())
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ShowMessageAsync("Информация", "Сохранение прошло успешно!");
                        });
                    }
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = false; });
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                    this.Dispatcher.Invoke(() => { this.ProgressRing.IsActive = false; });
                }
            });



        }

        private void MenuItem_OnClick_Update_All(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Update();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private async void MenuItem_OnClick_Add_Image(object sender, RoutedEventArgs e)
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
                await this.ShowMessageAsync("Информация", "Успешно!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            this.Close();

        }

        private void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateInfoAll();
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
                    MessageBox.Show(
                        "Пожалуйста, закончите создавать книгу, а потом пытайтесь её удалить. Для завершения создания книги нужно выделить другую книгу, либо создать новую.");
                    this.GridBooks.IsEnabled = true;
                    return;
                }


                var res = this.GridBooks.ItemsSource as List<Book>;

                var book = this._bookHelper.Get(((Book) GridBooks.SelectedItem).Id);
                if (book is null)
                {
                    res?.Remove(((Book)GridBooks.SelectedItem));
                }
                
                else
                {
                    if (this._readerRepository.Check(reader => reader.BookAndReaders.Any(dates => dates.Book.Equals(book))))
                    {
                        MessageBox.Show("Эта книга привязана к читателю, удаление невозможно");
                        
                    }
                    else
                    {
                        this._bookHelper.Delete(((Book)GridBooks.SelectedItem).Id);
                        res = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                    }
                  
                }

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = res;
                this.GridBooks.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
                this.GridBooks.IsEnabled = true;
            }

        }

        private void MenuItem_OnClick_Delete_Reader(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridReaders.SelectedIndex == -1 || (this.GridReaders.SelectedItem as Reader) is null)
                    return;

                try
                {
                    this.GridReaders.IsEnabled = false;
                }
                catch
                {
                    MessageBox.Show(
                        "Пожалуйста, закончите создавать подльзователя, а потом пытайтесь его удалить. Для завершения создания пользователя нужно выделить другого, либо создать нового.");
                    this.GridReaders.IsEnabled = true;
                    return;
                }


                var res = this.GridReaders.ItemsSource as List<Reader>;

                if (this._readerRepository.Get(((Reader)GridReaders.SelectedItem).Id) is null)
                {
                    res?.Remove(((Reader)this.GridReaders.SelectedItem));
                }

                else
                {
                    this._readerRepository.Delete(((Reader)this.GridReaders.SelectedItem).Id);
                    res = this._readerRepository.GetRange(this._fromReader, BeforeReader);
                }

                this.GridReaders.ItemsSource = null;
                this.GridReaders.ItemsSource = res;
                this.GridReaders.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
                this.GridReaders.IsEnabled = true;

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
                    MessageBox.Show(
                        "Пожалуйста, закончите создавать книгу, а потом перейдите в подробную информацию. Для завершения создания книги нужно выделить другую книгу, либо создать новую.");
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

                this.GridBooks.IsEnabled = false;



            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
                this.GridBooks.IsEnabled = false;
            }

        }

        private void MenuItem_OnClick_Theme_Dark(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Resources.Source =
                    new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/basedark.xaml");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void MenuItem_OnClick_Theme_Ligth(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Resources.Source =
                    new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/baselight.xaml");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
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

        private async void MenuItem_OnClick_Add_Genre(object sender, RoutedEventArgs e)
        {
            try
            {
                this.GridGenre.ItemsSource = null;
                var tmp = new AddGenre((GridBooks.SelectedItem as Book)?.Genre);
                tmp.ShowDialog();
                this.GridGenre.ItemsSource = (GridBooks.SelectedItem as Book)?.Genre.ToList();
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }
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


        private async void MenuItem_OnClick_Add_New_Genre(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewGenre();
                tmp.ShowDialog();
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Global_Delete_Genre(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new DeleteGenre();
                tmp.ShowDialog();
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Global_Delete_Type(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new DeleteType();
                tmp.ShowDialog();

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Global_Delete_Author(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new DeleteAuthor();
                tmp.ShowDialog();
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Global_Delete_Language(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new DeleteLanguage();
                tmp.ShowDialog();

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Global_Delete_Edition(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new DeleteEdition();
                tmp.ShowDialog();

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Global_Add_Language(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewLanguage();
                tmp.ShowDialog();

                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Add_New_Author(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewAuthor();
                tmp.ShowDialog();
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Add_New_Type(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewType();
                tmp.ShowDialog();
                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Add_New_Edition(object sender, RoutedEventArgs e)
        {
            try
            {

                var tmp = new AddNewEdition();
                tmp.ShowDialog();
                this.GridBooks.ItemsSource = null;
                this.GridBooks.ItemsSource = this._bookHelper.GetRange(this._fromBook, BeforeBook);
                if (tmp.Status)
                {
                    await this.ShowMessageAsync("Информация", "Успешно!");
                }

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
                    MessageBox.Show(
                        "Пожалуйста, закончите создавать пользователя, а потом перейдите в подробную информацию. Для завершения создания пользователя нужно выделить другого, либо начать создавать нового.");
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
                this.GridReaders.IsEnabled = true;
            }

        }

        private void MenuItem_OnClick_Info_Reader_Debtor(object sender, RoutedEventArgs e)
        {
            try
            {

                if ((this.GridReadersDebtor.SelectedItem as Reader) != null)
                {
                    var tmpList = new List<Reader> { ((Reader)this.GridReadersDebtor.SelectedItem) };
                    this.GridInfoLoginDebtor.ItemsSource = tmpList;
                    this.GridInfoDebtor.ItemsSource = tmpList.First().BookAndReaders;

                    this.FlyoutInfoDebtor.IsOpen = true;
                }

                this.GridReadersDebtor.IsEnabled = false;



            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void MenuItem_OnClick_Add_Book(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    if (((this.GridInfoLogin.ItemsSource as List<Reader>) ?? throw new InvalidOperationException())
                        .First() is null) return;
                    var tmp = new AddBook(((List<Reader>)this.GridInfoLogin.ItemsSource).First());
                    tmp.ShowDialog();


                    if (((Reader)this.GridReaders.SelectedItem) == null) return;

                    var tmpList = new List<Reader> { ((Reader)this.GridReaders.SelectedItem) };
                    this.GridInfoBooks.ItemsSource = null;
                    this.GridInfoBooks.ItemsSource = tmpList.First().BookAndReaders;
                    this.GridBooks.ItemsSource = null;
                    this.GridBooks.ItemsSource = _bookHelper.GetRange(this._fromBook, BeforeBook);
                    if (tmp.Status)
                    {
                        await this.ShowMessageAsync("Информация", "Успешно!");
                    }

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

        private async void MenuItem_OnClick_Take_Book(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    var reader = this.GridInfoBooks.SelectedItem as BookAndDates;
                    if (reader is null)
                    {
                        return;
                    }

                    var resPrice = ((DateTime.Now - reader.DateEnd.StartTime).Days) * reader.Book.Price;

                    await this.ShowMessageAsync("Информация", $"Человек должен заплатить - {resPrice}");

                    reader.Book.Count++;
                    this._bookAndReaderRepository.Delete(reader.Id);


                    this.GridBooks.ItemsSource = null;
                    this.GridBooks.ItemsSource = _bookHelper.GetRange(this._fromBook, BeforeBook);

                    if ((this.GridReaders.SelectedItem as Reader) == null) return;

                    this.GridInfoBooks.ItemsSource = null;
                    this.GridInfoBooks.ItemsSource = ((Reader)this.GridReaders.SelectedItem).BookAndReaders;

                    await this.ShowMessageAsync("Информация", "Успешно!");

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

        private async void MenuItem_OnClick_Take_Book_Debtor(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    if (!(this.GridInfoDebtor.SelectedItem is BookAndDates reader))
                    {
                        return;
                    }

                    var resPrice = ((DateTime.Now - reader.DateEnd.StartTime).Days) * reader.Book.Price;

                    await this.ShowMessageAsync("Информация", $"Человек должен заплатить - {resPrice}");

                    reader.Book.Count++;
                    this._bookAndReaderRepository.Delete(reader.Id);

                    this.UpdateBooks();

                    if ((this.GridReadersDebtor.SelectedItem as Reader) == null) return;

                    this.GridInfoDebtor.ItemsSource = null;
                    this.GridInfoDebtor.ItemsSource = ((Reader)this.GridReadersDebtor.SelectedItem).BookAndReaders;

                    await this.ShowMessageAsync("Информация", "Успешно!");

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

        #endregion

        #region Flyout

        private void FlyoutInfo_OnClosingFinished_Reader(object sender, RoutedEventArgs e)
        {
            try
            {

                ((Reader)this.GridReaders.SelectedItem).Password =
                    ((this.GridInfoLogin.ItemsSource as List<Reader>) ?? throw new InvalidOperationException()).First()
                    .Password;
                ((Reader)this.GridReaders.SelectedItem).BookAndReaders =
                    (((ICollection<BookAndDates>)this.GridInfoBooks.ItemsSource) ??
                     throw new InvalidOperationException());

                this.GridReaders.IsEnabled = true;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
                this.GridReaders.IsEnabled = true;
            }


        }

        private void FlyoutInfo_OnClosingFinished_Reader_Debtor(object sender, RoutedEventArgs e)
        {
            try
            {

                ((Reader)this.GridReadersDebtor.SelectedItem).Password =
                    ((this.GridInfoLoginDebtor.ItemsSource as List<Reader>) ?? throw new InvalidOperationException())
                    .First().Password;
                ((Reader)this.GridReadersDebtor.SelectedItem).BookAndReaders =
                    (((ICollection<BookAndDates>)this.GridInfoDebtor.ItemsSource) ??
                     throw new InvalidOperationException());

                this.UpdateDebtor();
                this.GridReadersDebtor.IsEnabled = true;


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
                this.GridReadersDebtor.IsEnabled = true;
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
                this.GridBooks.IsEnabled = true;
            }


        }

        #endregion

        #region Change

        private void GridReaders_OnAddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            try
            {
                e.NewItem = new Reader();
                ((Reader)e.NewItem).BookAndReaders = new List<BookAndDates>();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void RtFive_OnRatingChanged(object sender, RatingChangedEventArgs e)
        {
            try
            {
                if ((this.GridBooks.SelectedItem as Book) is null)
                {
                    return;
                }

                ((Book)this.GridBooks.SelectedItem).Rate = e.Value;
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
            try
            {
                if (!(GridBooks.CurrentItem is Book book && !((this.GridBooks.SelectedItem as Book) is null))) return;


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
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

            //Task.Factory.StartNew(() => { this.BeginInvoke(() => { this.UpdateBooks(); }); });
            //this.Update();





        }

        #endregion

        #region Search

        private void Selector_OnSelectionChanged_Book(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((sender as ComboBox) is null) return;

                switch (((ComboBox)sender).SelectedIndex)
                {
                    case 0: //одна книга
                        this.ComboBoxBooks.ItemsSource = new string[] { "Искать по названию" };
                        break;
                    case 1: //много книг
                        this.ComboBoxBooks.ItemsSource = new[]
                            {"Искать по автору", "Искать по жанру", "Искать по типу","Искать по названию(для типов)"};
                        break;
                    default:
                        return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "error");
            }



        }

        private void StartSearchBook_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TextBoxSearchBook.Text) || this.ComboBoxBooks.SelectedIndex == -1 ||
                    this.ComboBoxBooksSearchOne.SelectedIndex == -1)
                    return;
                List<Book> books;
                switch (this.ComboBoxBooksSearchOne.SelectedIndex)
                {
                    case 0:

                        switch (this.ComboBoxBooks.SelectedIndex)
                        {
                            case 0: //Искать по названию
                                var book = this._bookHelper.Get(book1 => book1.Name == this.TextBoxSearchBook.Text);
                                if (book is null)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    List<Book> tmpBooks = new List<Book> { book };
                                    this.GridBooks.ItemsSource = null;
                                    this.GridBooks.ItemsSource = tmpBooks;

                                }

                                break;
                            default: return;
                        }

                        break;
                    case 1:
                        switch (this.ComboBoxBooks.SelectedIndex)
                        {
                            case 0: //Искать по автору
                                books = this._bookHelper.GetAll(book =>
                                    book.Author.Any(author => author.Name == this.TextBoxSearchBook.Text));
                                if (books.Count < 1)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    this.GridBooks.ItemsSource = null;
                                    this.GridBooks.ItemsSource = books;
                                }

                                break;
                            case 1: //Искать по жанру
                                books = this._bookHelper.GetAll(book =>
                                    book.Genre.Any(genre => genre.Name == this.TextBoxSearchBook.Text));
                                if (books.Count < 1)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    this.GridBooks.ItemsSource = null;
                                    this.GridBooks.ItemsSource = books;
                                }

                                break;

                            case 2: //Искать по типу
                                books = this._bookHelper.GetAll(book => book.Type.Name == this.TextBoxSearchBook.Text);
                                if (books.Count < 1)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    this.GridBooks.ItemsSource = null;
                                    this.GridBooks.ItemsSource = books;
                                }

                                break;

                            case 3: //Искать по названию
                                books = this._bookHelper.GetAll(book => book.Name == this.TextBoxSearchBook.Text);
                                if (books.Count < 1)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    this.GridBooks.ItemsSource = null;
                                    this.GridBooks.ItemsSource = books;
                                }

                                break;
                            default: return;

                        }


                        break;
                    default:
                        return;
                }

                SettingSearchBooks();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private void StopSearchBook_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateBooks();
                SettingSearchBooks();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void ComboBoxSearchReaderOne_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((sender as ComboBox) is null) return;

                switch (((ComboBox)sender).SelectedIndex)
                {
                    case 0: //один пользователь
                        this.ComboBoxSearchReaderTwo.ItemsSource = new[] { "Искать по логину", "Искать по номеру" };
                        break;
                    case 1: //много пользователей
                        this.ComboBoxSearchReaderTwo.ItemsSource =
                            new[] { "Искать по книге", "Искать по дате сдачи книги" };
                        break;
                    default:
                        return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void SettingSearchReaders()
        {
            this.StartSearchReader.IsEnabled = !this.StartSearchReader.IsEnabled;
            this.GridReaders.CanUserAddRows = !this.GridReaders.CanUserAddRows;
            this.GridReaders.CanUserDeleteRows = !this.GridReaders.CanUserDeleteRows;
            this.ArrowLeftReader.IsEnabled = !this.ArrowLeftReader.IsEnabled;
            this.ArrowRightReader.IsEnabled = !this.ArrowRightReader.IsEnabled;
            this.ArrowLeftEndReader.IsEnabled = !this.ArrowLeftEndReader.IsEnabled;
            this.ArrowRightEndReader.IsEnabled = !this.ArrowRightEndReader.IsEnabled;
            this.StopSearchReaders.IsEnabled = !this.StopSearchReaders.IsEnabled;
            this.Menu.IsEnabled = !this.Menu.IsEnabled;
        }

        private void SettingSearchBooks()
        {
            this.StartSearchBook.IsEnabled = !this.StartSearchBook.IsEnabled;
            this.GridBooks.CanUserAddRows = !this.GridBooks.CanUserAddRows;
            this.GridBooks.CanUserDeleteRows = !this.GridBooks.CanUserDeleteRows;
            this.ArrowLeft.IsEnabled = !this.ArrowLeft.IsEnabled;
            this.ArrowRight.IsEnabled = !this.ArrowRight.IsEnabled;
            this.ArrowLeftEnd.IsEnabled = !this.ArrowLeftEnd.IsEnabled;
            this.ArrowRightEnd.IsEnabled = !this.ArrowRightEnd.IsEnabled;
            this.StopSearchBook.IsEnabled = !this.StopSearchBook.IsEnabled;
            this.Menu.IsEnabled = !this.Menu.IsEnabled;
        }

        private void StartSearchReader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TextBoxSearchReader.Text) ||
                    this.ComboBoxSearchReaderTwo.SelectedIndex == -1 ||
                    this.ComboBoxSearchReaderOne.SelectedIndex == -1)
                    return;
                switch (this.ComboBoxSearchReaderOne.SelectedIndex)
                {
                    case 0:

                        switch (this.ComboBoxSearchReaderTwo.SelectedIndex)
                        {
                            case 0: //Искать по логину
                                var tmpReader = this._readerRepository.Get(reader =>
                                    reader.Id.ToString() == this.TextBoxSearchReader.Text);
                                if (tmpReader is null)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    List<Reader> tmpReaders = new List<Reader> { tmpReader };
                                    this.GridReaders.ItemsSource = null;
                                    this.GridReaders.ItemsSource = tmpReaders;

                                }

                                break;

                            case 1: //искать по номеру
                                var tmpReaderPhone = this._readerRepository.Get(reader =>
                                    reader.Phone == this.TextBoxSearchReader.Text);
                                if (tmpReaderPhone is null)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    List<Reader> tmpReaders = new List<Reader> { tmpReaderPhone };
                                    this.GridReaders.ItemsSource = null;
                                    this.GridReaders.ItemsSource = tmpReaders;

                                }


                                break;

                            default: return;
                        }

                        break;
                    case 1:
                        List<Reader> readers;
                        switch (this.ComboBoxSearchReaderTwo.SelectedIndex)
                        {
                            case 0: //Искать по книге
                                readers = this._readerRepository.GetAll(reader =>
                                    reader.BookAndReaders.Any(book => book.Book.Name == this.TextBoxSearchReader.Text));
                                if (readers.Count < 1)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    this.GridReaders.ItemsSource = null;
                                    this.GridReaders.ItemsSource = readers;
                                }

                                break;
                            case 1: //Искать по времени сдачи
                                readers = this._readerRepository.GetAll(reader =>
                                    reader.BookAndReaders.Any(book =>
                                        book.DateEnd.EndTime.ToShortDateString() ==
                                        this.TextBoxSearchReader.Text));

                                MessageBox.Show(DateTime.Now.ToShortDateString());
                                if (readers.Count < 1)
                                {
                                    MessageBox.Show("Поиск не дал результатов!", "Error");
                                    return;
                                }
                                else
                                {
                                    this.GridReaders.ItemsSource = null;
                                    this.GridReaders.ItemsSource = readers;
                                }

                                break;
                            default: return;

                        }

                        break;
                    default:
                        return;

                }


                this.SettingSearchReaders();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private void StopSearchReaders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateReaders();
                this.SettingSearchReaders();
            }

            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        #endregion

        #region ButtonArrow

        private async void ArrowRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.Grid.IsEnabled = false;

                await Task.Factory.StartNew(delegate
                {
                    try
                    {

                        if (!this.SaveBook())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }

                        this._fromBook += 10;
                     
                        this.Dispatcher.Invoke(() =>
                        {

                            var books = _bookHelper.GetRange(this._fromBook, BeforeBook);
                            if (books.Count == 0)
                            {
                                MessageBox.Show("Последняя страница!", "Info");
                                this._fromBook -= 10;
                                 return;
                            }

                            foreach (var book in books)
                            {
                                book.Img = new Image{Source = ImageHelper.BytesToImage(book.Cover)};
                            }
                            this.GridBooks.ItemsSource = null;
                            this.GridBooks.ItemsSource = books;
                            this.NumPageBook.Count = $"{int.Parse(this.NumPageBook.Count) + 1}";

                        });


                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");

                    }
                });
                this.ProgressRing.IsActive = false;
                this.Grid.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
            

           

        }

        private async void ArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.Grid.IsEnabled = false;
                await Task.Factory.StartNew(delegate
                {
                    try
                    {
                        if ((this._fromBook - 10) < 0)
                        {
                            MessageBox.Show("Последняя страница!", "Info");
                            return;
                        }

                        if (!this.SaveBook())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }


                        this._fromBook -= 10;
                        

                        this.Dispatcher.Invoke(() =>
                        {
                            
                            this.UpdateBooks();

                            this.NumPageBook.Count = $"{int.Parse(this.NumPageBook.Count) - 1}";

                        });

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }
                });

                this.ProgressRing.IsActive = false;
                this.Grid.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
            
        }

        private async void ArrowRight_Reader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.GridReader.IsEnabled = false;

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (!this.SaveReader())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }

                        this._fromReader += 10;
                        

                        this.Dispatcher.Invoke(() =>
                        {
                            var reader = this._readerRepository.GetRange(this._fromReader, BeforeReader);

                            if (reader.Count == 0)
                            {
                                MessageBox.Show("Последняя страница!", "Info");
                                this._fromReader -= 10;
                                
                                return;
                            }

                            this.GridReaders.ItemsSource = null;
                            this.GridReaders.ItemsSource = reader;
                            this.NumPageReader.Count = $"{int.Parse(this.NumPageReader.Count) + 1}";
                        });

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }
                    

                   
                });
                this.ProgressRing.IsActive = false;
                this.GridReader.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }


        }

        private async void ArrowLeft_Reader_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                this.ProgressRing.IsActive = true;
                this.GridReader.IsEnabled = false;

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if ((this._fromReader - 10) < 0)
                        {
                            MessageBox.Show("Последняя страница!", "Info");
                            return;
                        }

                        if (!this.SaveReader())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }


                        this._fromReader -= 10;
                       


                        this.Dispatcher.Invoke(() =>
                        {
                            this.GridReaders.ItemsSource = null;
                            this.GridReaders.ItemsSource =
                            this._readerRepository.GetRange(this._fromReader, BeforeReader);

                            this.NumPageReader.Count = $"{int.Parse(this.NumPageReader.Count) - 1}";
                        });

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }


                });


                this.ProgressRing.IsActive = false;
                this.GridReader.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }


        private async void ArrowLeftEnd_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                this.ProgressRing.IsActive = true;
                this.Grid.IsEnabled = false;

                await Task.Factory.StartNew(() =>
                {
                    try
                    {

                        if (this._fromBook == 0)
                        {
                            MessageBox.Show("Вы на первой странице!", "Error");
                            return;
                        }

                        this._fromBook = 0;
                      

                        if (!this.SaveBook())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            this.UpdateBooks();

                            this.NumPageBook.Count = "1";
                        });


                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }
                });


                this.ProgressRing.IsActive = false;
                this.Grid.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }



        }

        private async void ArrowRightEnd_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.Grid.IsEnabled = false;
                await Task.Factory.StartNew(() =>
                {
                    try
                    {


                        if (!this.SaveBook())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }

                        var resBefore = (this._bookHelper.GetSize());
                        var rest = resBefore % 10;

                        if (rest == 0)
                        {
                            this._fromBook = resBefore - 10;
                            
                        }

                        else
                        {
                            this._fromBook = resBefore - rest;
                            
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            this.UpdateBooks();

                            this.NumPageBook.Count = $"{((this._fromBook + 10) / 10)}";
                            
                        });


                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }
                });

                this.ProgressRing.IsActive = false;
                this.Grid.IsEnabled = true;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }

        }

        private async void ArrowLeftEndReader_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.GridReader.IsEnabled = false;


                await Task.Factory.StartNew(() =>
                {
                    try
                    {

                        if (this._fromReader == 0)
                        {
                            MessageBox.Show("Вы на первой странице!", "Error");
                            return;
                        }

                        this._fromReader = 0;
                       

                        if (!this.SaveReader())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            this.GridReaders.ItemsSource = null;

                            this.GridReaders.ItemsSource =
                                this._readerRepository.GetRange(this._fromReader, BeforeReader);
                            this.NumPageReader.Count = "1";

                        });

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }
                });



                this.ProgressRing.IsActive = false;
                this.GridReader.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
           
        }

        private async void ArrowRightEndReader_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ProgressRing.IsActive = true;
                this.GridReader.IsEnabled = false;

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (!this.SaveReader())
                        {
                            MessageBox.Show("Невозможно сохранить текущую страницу!");
                            return;
                        }

                        var resBefore = (this._readerRepository.GetSize());
                        var rest = resBefore % 10;

                        if (rest == 0)
                        {
                            this._fromReader = resBefore - 10;
                           // this._beforeReader = resBefore;
                        }

                        else
                        {
                            this._fromReader = resBefore - rest;
                           // this._beforeReader = resBefore + (10 - (rest - 10 * (rest / 10)));
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            this.GridReaders.ItemsSource = null;
                            this.GridReaders.ItemsSource =
                                this._readerRepository.GetRange(this._fromReader, BeforeReader);

                            this.NumPageReader.Count = $"{((this._fromReader + 10) / 10)}";
                           
                          
                        });

                        
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "Error");
                    }
                   
                });


                this.ProgressRing.IsActive = false;
                this.GridReader.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }
        #endregion
    }
}
