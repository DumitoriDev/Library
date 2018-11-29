using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookAndReader
    {
        public int Id { get; set; }
        public ICollection<Book> Books { get; set; }
        public Reader ReaderId { get; set; }
        public ICollection<DateTime> DateEnd { get; set; }
    }

    class BookAndReaderRepository : IRepository<BookAndReader>
    {
        private static BookAndReaderRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private BookAndReaderRepository() { }
        public static BookAndReaderRepository GetInstance()
        {
            if (instance == null)
                instance = new BookAndReaderRepository();
            return instance;
        }
        public void Add(BookAndReader bookAndReader)
        {
            baseContext.BookAndReaders.Add(bookAndReader);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.BookAndReaders.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public BookAndReader Get(int id)
        {
            return baseContext.BookAndReaders.FirstOrDefault(bookAndReader => bookAndReader.Id == id);
        }

        public IEnumerable<BookAndReader> GetAll()
        {
            return baseContext.BookAndReaders.ToList();
        }

        public void Update(BookAndReader newBookAndReader)
        {
            BookAndReader changeable = Get(newBookAndReader.Id);
            if (changeable != null)
            {
                changeable.ReaderId = newBookAndReader.ReaderId;
                changeable.Books = newBookAndReader.Books;
                changeable.DateEnd = newBookAndReader.DateEnd;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}