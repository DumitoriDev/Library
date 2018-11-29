using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
    }

    class AuthorRepository : IRepository<Author>
    {
        private static AuthorRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private AuthorRepository() { }
        public static AuthorRepository GetInstance()
        {
            if (instance == null)
                instance = new AuthorRepository();
            return instance;
        }
        public void Add(Author author)
        {
            baseContext.Authors.Add(author);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Authors.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Author Get(int id)
        {
            return baseContext.Authors.FirstOrDefault(author => author.Id == id);
        }

        public IEnumerable<Author> GetAll()
        {
            return baseContext.Authors.ToList();
        }

        public void Update(Author newAuthor)
        {
            Author changeable = Get(newAuthor.Id);
            if(changeable!=null)
            {
                changeable.Name = newAuthor.Name;
                changeable.LastName = newAuthor.LastName;
                changeable.Patronymic = newAuthor.Patronymic;
                changeable.Birth = newAuthor.Birth;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}
