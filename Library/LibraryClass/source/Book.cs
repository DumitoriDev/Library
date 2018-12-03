using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;

using System.Drawing;
using System.Windows.Controls;

namespace LibraryClass
{
    public class Book
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> AuthorId { get; set; }
        public int Pages { get; set; }
        public virtual ICollection<Genre> GenreId { get; set; }
        public Edition EditionId { get; set; }
        public DateTime Date { get; set; }
        public Language LanguageId { get; set; }
        public Type TypeId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public byte[] Cover { get; set; }
        public string Desc { get; set; }
        public int Rate { get; set; }

        [NotMapped] public System.Windows.Controls.Image Img { get; set; } = new System.Windows.Controls.Image();

    }


}
