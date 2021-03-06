﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using System.Windows.Threading;
using LibraryClass.source;

namespace LibraryClass
{
    public class BookRepository : IRepository<Book>
    {

        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        public List<Book> GetAll()
        {

            //var langs = new LanguageRepository().GetAll();
            //var editions = new EditionRepository().GetAll();
            //var types = new TypeRepository().GetAll();
            _baseContext.Books.LoadAsync().Wait();
            var books = _baseContext.Books.Local.ToList();

           
            //foreach (var book in books)
            //{
            //    book.Languages = langs;
            //    book.Types = types;
            //    book.Editions = editions;
            //}

            return books;
        }

        public void Add(Book book)
        {
            _baseContext.Books.Add(book);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var res = Get(id);
            _baseContext.Books.Remove(res);
            _baseContext.SaveChanges();
        }

        public void UpdateAll(List<Book> books)
        {
            foreach (var book in books)
            {
                this.Update(book);
            }
        }
        
        public List<Book> GetRange(int from, int before)
        {
           
            var langs = new LanguageRepository().GetAll();
            var editions = new EditionRepository().GetAll();
            var types = new TypeRepository().GetAll();

            
            var books = _baseContext.Books.OrderBy(book => book.Id).Skip(from).Take(before).ToList();
            foreach (var book in books)
            {
               book.Languages = langs;
               book.Types = types;
               book.Editions = editions;
                
            }

            return books;
        }

        public Book Get(int id)
        {

            return _baseContext.Books.FirstOrDefault(book => book.Id == id);
        }
        
        public Book Get(Func<Book, bool> func)
        {
            return _baseContext.Books.FirstOrDefault(func);
        }

        

        public DbSet<Book> GetAll1()
        {
            lock (_baseContext)
            {


                var langs = new LanguageRepository().GetAll();
                var editions = new EditionRepository().GetAll();
                var types = new TypeRepository().GetAll();
                
                var books = _baseContext.Books;
                //foreach (var book in books)
                //{
                //    book.Languages = langs;
                //    book.Types = types;
                //    book.Editions = editions;
                //}


                return _baseContext.Books;
            }
        }


        public List<Book> GetAll(Func<Book, bool> func)
        {
            var langs = new LanguageRepository().GetAll();
            var editions = new EditionRepository().GetAll();
            var types = new TypeRepository().GetAll();
            var books = _baseContext.Books.Where(func).ToList();
            foreach (var book in books)
            {
                book.Languages = langs;
                book.Types = types;
                book.Editions = editions;

                book.Img.Source = ImageHelper.BytesToImage(book.Cover);
            }

            return books;
        }



        public List<Book> GetAllImg()
        {

            var tmpBooks = _baseContext.Books.ToList();
            foreach (var t in tmpBooks)
            {
                t.Img.Source = ImageHelper.BytesToImage(t.Cover);
            }

            return tmpBooks;
        }

        public void Update(Book newBook)
        {

            var changeable = Get(newBook.Id);

            if (changeable == null)
            {
                this.Add(newBook);
                return;
            }

            changeable.Name = newBook.Name;
            changeable.Type = newBook.Type;
            changeable.Language = newBook.Language;
            changeable.Pages = newBook.Pages;
            changeable.Cover = newBook.Cover;
            changeable.Date = newBook.Date;
            changeable.Desc = newBook.Desc;
            changeable.Edition = newBook.Edition;
            changeable.Count = newBook.Count;
            changeable.Price = newBook.Price;


            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();

        }

        public static bool Check(Book book)
        {
            return book.Name != ""
                   && TypeRepository.Check(book.Type)
                   && book.Date > 0
                   && EditionRepository.Check(book.Edition)
                   && book.Desc != ""
                   && LanguageRepository.Check(book.Language)
                   && AuthorRepository.CheckAll(book.Author)
                   && GenreRepository.CheckAll(book.Genre);


        }



        public bool Check(Func<Book, bool> func)
        {
            return this._baseContext.Books.Any(func);
        }

        public int GetSize()
        {
            return this._baseContext.Books.Count();
        }
    }
}
