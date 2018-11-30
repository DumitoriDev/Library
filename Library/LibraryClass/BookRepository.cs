using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
   public class BookRepository : IRepository<Book>
    {
        private static BookRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private BookRepository() { }

        public static BookRepository GetInstance()
        {
            return _instance ?? (_instance = new BookRepository());
        }

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

        public Book Get(int id)
        {
            return _baseContext.Books.FirstOrDefault(book => book.Id == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _baseContext.Books.ToList();
        }

        public void Update(Book newBook)
        {
            var changeable = Get(newBook.Id);
            if (changeable == null) return;

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
