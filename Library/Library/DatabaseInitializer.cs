using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class DatabaseInitializer: DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext ctx)
        {
            //var tmpCategory = new Category
            //{
            //    Name = "Milk products",
            //    Products = new List<Product>
            //    {
            //        new Product
            //        {
            //            Name = "Milk",
            //            Price = 100
            //        },
            //        new Product
            //        {
            //            Name = "Kefir",
            //            Price = 70
            //        }

            //    }

            //};

            //ctx.Entry(tmpCategory).State = EntityState.Added;
            //ctx.SaveChanges();
        }
    }
}
