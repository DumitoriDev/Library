using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookAndReader
    {
        public int Id { get; set; }
        public ICollection<Book> Books { get; set; }
        public Reader ReaderId{ get; set; }
        public ICollection<DateTime > DateEnd { get; set; }
    }
}
