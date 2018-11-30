using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Library
{
    public class DataBaseContext:DbContext
    {
        private DataBaseContext() : base("connStr") { }
        private static DataBaseContext instance;
        public static DataBaseContext GetInstance()
        {
            if(instance==null)
            {
                instance = new DataBaseContext();
            }
            return instance;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<BookAndReader> BookAndReaders { get; set; }
    }
}
