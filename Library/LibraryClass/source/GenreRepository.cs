﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class GenreRepository : IRepository<Genre>
    {

        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();

        public int GetSize()
        {
            return this._baseContext.Genres.Count();
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

        public Genre Get(Func<Genre, bool> func)
        {
            return _baseContext.Genres.FirstOrDefault(func);
        }

        public List<Genre> GetAll()
        {
            return _baseContext.Genres.ToList();
        }

        public void Update(Genre newGenre)
        {
            var changeable = Get(newGenre.Id);
            if (changeable == null) return;
            changeable.Name = newGenre.Name;
            changeable.Books = newGenre.Books;
            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }


        public bool Check(Func<Genre, bool> func)
        {
            return _baseContext.Genres.All(func);
        }

        public static bool Check(Genre genre)
        {
            return genre.Name != "";
        }

        public static bool CheckAll(IEnumerable<Genre> genre)
        {
            return  genre.Any() && genre.All(Check);
        }
    }
}
