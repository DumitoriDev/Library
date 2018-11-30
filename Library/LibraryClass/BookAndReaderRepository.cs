using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    internal class BookAndReaderRepository : IRepository<BookAndReader>
    {
        private static BookAndReaderRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private BookAndReaderRepository() { }
        public static BookAndReaderRepository GetInstance()
        {
            if (_instance == null)
                _instance = new BookAndReaderRepository();
            return _instance;
        }
        public void Add(BookAndReader bookAndReader)
        {
            _baseContext.BookAndReaders.Add(bookAndReader);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.BookAndReaders.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public BookAndReader Get(int id)
        {
            return _baseContext.BookAndReaders.FirstOrDefault(bookAndReader => bookAndReader.Id == id);
        }

        public IEnumerable<BookAndReader> GetAll()
        {
            return _baseContext.BookAndReaders.ToList();
        }

        public void Update(BookAndReader newBookAndReader)
        {
            var changeable = Get(newBookAndReader.Id);
            if (changeable != null)
            {
                changeable.ReaderId = newBookAndReader.ReaderId;
                changeable.Books = newBookAndReader.Books;
                changeable.DateEnd = newBookAndReader.DateEnd;

                _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                _baseContext.SaveChanges();
            }
        }
    }
}
