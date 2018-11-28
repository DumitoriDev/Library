using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Author_id { get; set; }
        public int Pages { get; set; }
        public int Genre_id { get; set; }
        public int Edition_id { get; set; }
        public DateTime Date { get; set; }
        public int Language_id { get; set; }
        public int Type_id { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public Image Cover { get; set; }
        public string Desc { get; set; }
    }
}
