using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> AuthorId { get; set; }
        public int Pages { get; set; }
        public virtual ICollection<Genre> GenreId { get; set; }
        public Edition EditionId { get; set; }
        public DateTime Date { get; set; }
        public Language LanguageId { get; set; }
        public Type TypeId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public byte[] Cover { get; set; }
        public string Desc { get; set; }
        public int Rate { get; set; }
    }

    class BookRepository : IRepository<Book>
    {
        private static BookRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private BookRepository() { }

        public static BookRepository GetInstance()
        {
            if (instance == null)
                instance = new BookRepository();
            return instance;
        }

        public void Add(Book book)
        {
            baseContext.Books.Add(book);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Books.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Book Get(int id)
        {
            return baseContext.Books.FirstOrDefault(book => book.Id == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return baseContext.Books.ToList();
        }

        public void Update(Book newBook)
        {
            Book changeable = Get(newBook.Id);
            if(changeable!=null)
            {
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

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }

        }
    }
}
