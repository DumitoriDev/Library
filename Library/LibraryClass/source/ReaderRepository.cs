using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class ReaderRepository : IRepository<Reader>
    {

        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();

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

        public Reader Get(Func<Reader, bool> func)
        {
            return _baseContext.Readers.FirstOrDefault(func);
        }

        public Reader Get(int id)
        {
            return _baseContext.Readers.FirstOrDefault(reader => reader.Id == id);
        }

        public List<Reader> GetAll()
        {
            return _baseContext.Readers.ToList();
        }

        public void Update(Reader newReader)
        {
            var changeable = this.Get(newReader.Id);
            if (changeable == null)
            {
                this.Add(newReader);
                return;
            }
            changeable.Name = newReader.Name;
            changeable.LastName = newReader.LastName;
            changeable.NumberDocument = newReader.NumberDocument;
            changeable.Password = newReader.Password;
            changeable.Phone = newReader.Phone;
            changeable.BookAndReaders = newReader.BookAndReaders;
            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }

        public void UpdateAll(List<Reader> readers)
        {
            foreach (var reader in readers)
            {
                this.Update(reader);
            }
        }

        public static bool Check(Reader reader)
        {
            return !string.IsNullOrEmpty(reader.LastName ) && !string.IsNullOrEmpty(reader.Name) && !string.IsNullOrEmpty(reader.Password) && !string.IsNullOrEmpty(reader.NumberDocument);
        }

        public static bool CheckAll(IEnumerable<Reader> reader)
        {
            return reader.All(Check);
        }

    }
}
