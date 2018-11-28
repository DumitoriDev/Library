using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

//DropCreateDatabaseIfModelChanges<DataBaseContext>

namespace Library
{
    class DatabaseInitializer: DropCreateDatabaseAlways<DataBaseContext>
    {
        protected override void Seed(DataBaseContext ctx)
        {
            var tmpBook = new Book
            {
                Count = 2,
                AuthorId = new List<Author>
                {
                    new Author
                    {
                        Birth = new DateTime(2018,12,12),
                        LastName = "Lox",
                        Name = "Pasha",
                        Patronymic = "Pashovich"
                    }

                },
                Cover = new byte[10],
                Date = new DateTime(2018,12,12),
                Desc = "prosto Book",
                EditionId = new Edition
                {
                    Name = "Black"
                },
                GenreId = new List<Genre>
                {
                    new Genre
                    {
                        Name = "Roman"
                    }
                },
                LanguageId = new Language
                {
                    Name = "Russian"
                },
                Name = "Black and Red",
                TypeId = new Type
                {
                    Name = "Book"
                },
                Pages = 1000,
                Price = 50,
                Rate = 3

            };
            var tmpReader = new Reader
            {
                Books = 1,
                LastName = "Petrenko",
                Name = "Masha",
                NumberDocument = "123",
                Password = "tmp",
                Phone = "38923"
            };
            var tmpBookAndReader = new BookAndReader
            {
                Books = new List<Book> {tmpBook},
                DateEnd = new List<DateTime> {new DateTime(2018, 12, 12) },
                ReaderId = tmpReader

            };
            //var tmpCategory = new Category
            //{
            //    Name = "Milk products",
            //    Products = new List<Product>
            //    {
            //        new Product
            //        {
            //            Name = "Milk",
            //            Price = 100
            //        },
            //        new Product
            //        {
            //            Name = "Kefir",
            //            Price = 70
            //        }

            //    }

            //};

           
            ctx.Entry(tmpBook).State = EntityState.Added;
            ctx.Entry(tmpReader).State = EntityState.Added;
            ctx.Entry(tmpBookAndReader).State = EntityState.Added;
            ctx.SaveChanges();
        }
    }
}
