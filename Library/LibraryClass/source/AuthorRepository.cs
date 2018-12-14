using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass.source
{
    public class AuthorRepository : IRepository<Author>
    {
     
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        public AuthorRepository() { }
       
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

        public Author Get(Func<Author, bool> func)
        {
            return _baseContext.Authors.FirstOrDefault(func);
        }

        //public List<Author> GetBookAuthor(Book book)
        //{
        //    return this._baseContext.Authors.Where(author => author.Books.Any(book1 => book.Id ) )
        //}

        public List<Author> GetAll()
        {
            return _baseContext.Authors.ToList();
        }

        public bool Check(Func<Author, bool> func)
        {
            return _baseContext.Authors.All(func);
        }

        public static bool Check(Author author)
        {
            return !string.IsNullOrEmpty(author.Name);
        }

        public static bool CheckAll(IEnumerable<Author> authors)
        {
            return authors.Any() && authors.All(Check);
        }

        public void Update(Author newAuthor)
        {
            var changeable = Get(newAuthor.Id);
            if (changeable == null) return;
            changeable.Name = newAuthor.Name;
            changeable.Books = newAuthor.Books;
           

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }
    }
}
