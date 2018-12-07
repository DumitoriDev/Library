using System;
using System.Collections.Generic;
using System.Linq;


namespace LibraryClass
{
    public class BookAndReader
    {
        public int Id { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public Reader ReaderId { get; set; }
        public virtual ICollection<UntilDate> DateEnd { get; set; }
    }

   
}