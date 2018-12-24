using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Net.Mime;

using System.Drawing;
using System.IO;
using System.Reflection;
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

        public virtual ICollection<Author> Author { get; set; } 

        public int Pages { get; set; }

        public virtual ICollection<Genre> Genre { get; set; } 

        public virtual Edition Edition { get; set; } 

        public int Date { get; set; }

        public virtual Language Language { get; set; }

        public virtual Type Type { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public byte[] Cover { get; set; }

        public string Desc { get; set; }

        public int Rate { get; set; }





        [NotMapped] public Image Img { get; set; } 

        [NotMapped] public List<Type> Types { get; set; }

        [NotMapped] public List<Edition> Editions { get; set; }

        [NotMapped] public List<Language> Languages { get; set; }

       

    }


}
