using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryClass
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
    }

    internal class AuthorRepository : IRepository<Author>
    {
        private static AuthorRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private AuthorRepository() { }
        public static AuthorRepository GetInstance()
        {
            return _instance ?? (_instance = new AuthorRepository());
        }
        public void Add(Author author)
        {
            _baseContext.Authors.Add(author);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.Authors.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public Author Get(int id)
        {
            return _baseContext.Authors.FirstOrDefault(author => author.Id == id);
        }

        public IEnumerable<Author> GetAll()
        {
            return _baseContext.Authors.ToList();
        }

        public void Update(Author newAuthor)
        {
            var changeable = Get(newAuthor.Id);
            if (changeable == null) return;
            changeable.Name = newAuthor.Name;
            changeable.LastName = newAuthor.LastName;
            changeable.Patronymic = newAuthor.Patronymic;
            changeable.Birth = newAuthor.Birth;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }
    }
}
