using System;
using System.Collections.Generic;
using System.Linq;


namespace LibraryClass
{
    public class BookAndDates
    {
        public int Id { get; set; }
       
        public virtual ICollection<Reader> Readers { get; set; }
        public virtual Book Book { get; set; }
        public virtual UntilDate DateEnd { get; set; }
    }

   
}