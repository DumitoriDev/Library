using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class ReaderRepository : IRepository<Reader>
    {
        private static ReaderRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private ReaderRepository() { }
        public static ReaderRepository GetInstance()
        {
            return _instance ?? (_instance = new ReaderRepository());
        }
        public void Add(Reader reader)
        {
            _baseContext.Readers.Add(reader);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.Readers.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public Reader Get(int id)
        {
            return _baseContext.Readers.FirstOrDefault(reader => reader.Id == id);
        }

        public IEnumerable<Reader> GetAll()
        {
            return _baseContext.Readers.ToList();
        }

        public void Update(Reader newReader)
        {
            var changeable = Get(newReader.Id);
            if (changeable == null) return;
            changeable.Name = newReader.Name;
            changeable.LastName = newReader.LastName;
            changeable.NumberDocument = newReader.NumberDocument;
            changeable.Password = newReader.Password;
            changeable.Phone = newReader.Phone;
            changeable.Books = newReader.Books;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }
    }
}
