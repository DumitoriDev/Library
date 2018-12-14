using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    /// <summary>
    /// 
    /// </summary>
    public class EditionRepository : IRepository<Edition>
    {
       
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        public EditionRepository() { }
       
        public void Add(Edition edition)
        {
            _baseContext.Editions.Add(edition);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.Editions.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public Edition Get(Func<Edition, bool> func)
        {
            return _baseContext.Editions.FirstOrDefault(func);
        }


        public Edition Get(int id)
        {
            return _baseContext.Editions.FirstOrDefault(edition => edition.Id == id);
        }

        public List<Edition> GetAll()
        {
            return _baseContext.Editions.ToList();
        }

        public void Update(Edition newEdition)
        {
            var changeable = Get(newEdition.Id);
            if (changeable == null) return;
            changeable.Name = newEdition.Name;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }

        public static bool Check(Edition edition )
        {
            return !string.IsNullOrEmpty(edition.Name);
        }


        public static bool CheckAll(IEnumerable<Edition> editions)
        {
            return editions.All(Check);
        }

    }
}
