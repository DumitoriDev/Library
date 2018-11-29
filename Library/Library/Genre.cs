using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    class GenreRepository : IRepository<Genre>
    {
        private static GenreRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private GenreRepository() { }
        public static GenreRepository GetInstance()
        {
            if (instance == null)
                instance = new GenreRepository();
            return instance;
        }
        public void Add(Genre genre)
        {
            baseContext.Genres.Add(genre);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Genres.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Genre Get(int id)
        {
            return baseContext.Genres.FirstOrDefault(genre => genre.Id == id);
        }

        public IEnumerable<Genre> GetAll()
        {
            return baseContext.Genres.ToList();
        }

        public void Update(Genre newGenre)
        {
            Genre changeable = Get(newGenre.Id);
            if (changeable != null)
            {
                changeable.Name = newGenre.Name;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}
