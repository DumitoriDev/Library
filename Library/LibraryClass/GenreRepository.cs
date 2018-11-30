using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class GenreRepository : IRepository<Genre>
    {
        private static GenreRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private GenreRepository() { }
        public static GenreRepository GetInstance()
        {
            return _instance ?? (_instance = new GenreRepository());
        }
        public void Add(Genre genre)
        {
            _baseContext.Genres.Add(genre);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.Genres.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public Genre Get(int id)
        {
            return _baseContext.Genres.FirstOrDefault(genre => genre.Id == id);
        }

        public IEnumerable<Genre> GetAll()
        {
            return _baseContext.Genres.ToList();
        }

        public void Update(Genre newGenre)
        {
            var changeable = Get(newGenre.Id);
            if (changeable == null) return;
            changeable.Name = newGenre.Name;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }
    }
}
