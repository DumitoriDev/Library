using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Reader
    {
        public int Id { get; set; }

        public string Password { get; set; }
        public int Books { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NumberDocument { get; set; }
        public string Phone { get; set; }
    }


    class ReaderRepository : IRepository<Reader>
    {
        private static ReaderRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private ReaderRepository() { }
        public static ReaderRepository GetInstance()
        {
            if (instance == null)
                instance = new ReaderRepository();
            return instance;
        }
        public void Add(Reader reader)
        {
            baseContext.Readers.Add(reader);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Readers.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Reader Get(int id)
        {
            return baseContext.Readers.FirstOrDefault(reader => reader.Id == id);
        }

        public IEnumerable<Reader> GetAll()
        {
            return baseContext.Readers.ToList();
        }

        public void Update(Reader newReader)
        {
            Reader changeable = Get(newReader.Id);
            if (changeable != null)
            {
                changeable.Name = newReader.Name;
                changeable.LastName = newReader.LastName;
                changeable.NumberDocument = newReader.NumberDocument;
                changeable.Password = newReader.Password;
                changeable.Phone = newReader.Phone;
                changeable.Books = newReader.Books;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}
