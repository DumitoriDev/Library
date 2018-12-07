using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
   public class BookRepository : IRepository<Book>
    {
       
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        public BookRepository() { }

       

        public void Add(Book book)
        {
            _baseContext.Books.Add(book);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.Books.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public void UpdateAll(List<Book> tmpbooks)
        {
            foreach (var book in tmpbooks)
            {
                this.Update(book);
            }
        }
        public Book Get(int id)
        {
            return _baseContext.Books.FirstOrDefault(book => book.Id == id);
        }
        public Book Get(Func<Book, bool> func)
        {
            return _baseContext.Books.FirstOrDefault(func);
        }
        public List<Book> GetAll()
        {
            return _baseContext.Books.ToList();
        }

        public List<Book> GetAllImg()
        {
            var tmpBooks = _baseContext.Books.ToList();
            foreach (var t in tmpBooks)
            {
                t.Img.Source = ImageHelper.ByteToImage(t.Cover);
            }

            return tmpBooks;
        }

        public void Update(Book newBook)
        {
            
            var changeable = Get(newBook.Id);

            if (changeable == null)
            {
                this.Add(newBook);
                return;
            }

            changeable.Name = newBook.Name;
            changeable.AuthorId = newBook.AuthorId;
            changeable.TypeId = newBook.TypeId;
            changeable.GenreId = newBook.GenreId;
            changeable.LanguageId = newBook.LanguageId;
            changeable.Pages = newBook.Pages;
            changeable.Cover = newBook.Cover;
            changeable.Date = newBook.Date;
            changeable.Desc = newBook.Desc;
            changeable.EditionId = newBook.EditionId;
            changeable.Count = newBook.Count;
            changeable.Price = newBook.Price;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();

        }
    }
}
