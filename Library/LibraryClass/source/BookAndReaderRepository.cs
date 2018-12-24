using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class BookAndReaderRepository : IRepository<BookAndDates>
    {
        
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();

        public BookAndReaderRepository() { }
        
        public void Add(BookAndDates bookAndDates)
        {
            _baseContext.BookAndReaders.Add(bookAndDates);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.BookAndReaders.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public BookAndDates Get(int id)
        {
            return _baseContext.BookAndReaders.FirstOrDefault(bookAndReader => bookAndReader.Id == id);
        }

        public BookAndDates Get(Func<BookAndDates, bool> func)
        {
            return _baseContext.BookAndReaders.FirstOrDefault(func);
        }

        public List<BookAndDates> GetAll()
        {
            return _baseContext.BookAndReaders.ToList();
        }

        public void Update(BookAndDates bookAndDates)
        {
            var changeable = Get(bookAndDates.Id);
            if (changeable == null) return;
            
            changeable.Readers = bookAndDates.Readers;
            changeable.DateEnd = bookAndDates.DateEnd;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }

        public static bool Check(BookAndDates bookAndDates)
        {
            return ReaderRepository.CheckAll(bookAndDates.Readers) && UntilDateRepository.Check(bookAndDates.DateEnd);
        }
        
        public static bool Check(IEnumerable<BookAndDates >bookAndReaders)
        {
            return bookAndReaders.All(Check);
        }



    }
}
