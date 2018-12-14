using System;
using System.Collections.Generic;
using System.Data.Entity;
//DropCreateDatabaseIfModelChanges<DataBaseContext>
namespace LibraryClass
{
    internal class DatabaseInitializer: DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext ctx)
        {
            var tmpReader = new Reader
            {
               
                LastName = "Petrenko",
                Name = "Masha",
                NumberDocument = "123",
                Password = "tmp",
                Phone = "38923"
            };
            var tmpBook = new Book
            {
                Count = 2,
                
                Cover = new byte[10],
                Date = 2018,
                Desc = "prosto Book",
                Edition = new Edition
                {
                    Name = "Black"
                },
                
                Language = new Language
                {
                    Name = "Russian"
                },
                Name = "Black and Red",
                Type = new Type
                {
                    Name = "Book"
                },
               
                Pages = 1000,
                Price = 50,
                Rate = 3

            };
           
            //var tmpBookAndReader = new BookAndReader
            //{
                
            //    DateEnd = new List<UntilDate> { new UntilDate{EndTime = new DateTime(2012,12,12)}},
                
            //    Reader = tmpReader

            //};
           

            ctx.Entry(tmpBook).State = EntityState.Added;
            ctx.Entry(tmpReader).State = EntityState.Added;
            //ctx.Entry(tmpBookAndReader).State = EntityState.Added;
            ctx.SaveChanges();
        }

    }
}
