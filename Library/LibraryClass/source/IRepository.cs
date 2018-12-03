using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LibraryClass
{
    internal interface IRepository<T> where T : class
    {
        T Get(int id);
        void Update(T item);
        void Delete(int id);
        void Add(T item);
        IEnumerable<T> GetAll();
    }

}
