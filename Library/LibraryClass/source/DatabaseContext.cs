using System.Data.Entity;

namespace LibraryClass
{
    public class DataBaseContext:DbContext
    {
        public static string connStr;
        private DataBaseContext() : base(connStr) { }
        private static DataBaseContext _instance;
        public static DataBaseContext GetInstance()
        {
            return _instance ?? (_instance = new DataBaseContext());
        }

        static DataBaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<BookAndReader> BookAndReaders { get; set; }
        public DbSet<UntilDate> UntilDate { get; set; }
    }
}
