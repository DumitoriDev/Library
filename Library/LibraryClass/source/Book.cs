using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Net.Mime;

using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;


namespace LibraryClass
{

    public class Book
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> AuthorId { get; set; } = new List<Author>();
        public int Pages { get; set; }
        public virtual ICollection<Genre> GenreId { get; set; } = new List<Genre>();
        public virtual Edition EditionId { get; set; } = new Edition();
        public DateTime Date { get; set; } = new DateTime();
        public virtual Language LanguageId { get; set; } = new Language();
        public virtual Type TypeId { get; set; } = new Type();
        public int Count { get; set; }
        public decimal Price { get; set; }
        public byte[] Cover { get; set; }
        public string Desc { get; set; }
        public int Rate { get; set; }

        [NotMapped] public Image Img { get; set; } = new Image();

       
      


    }


}
