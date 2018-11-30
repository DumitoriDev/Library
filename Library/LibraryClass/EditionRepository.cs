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
        private static EditionRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private EditionRepository() { }
        public static EditionRepository GetInstance()
        {
            return _instance ?? (_instance = new EditionRepository());
        }
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

        public Edition Get(int id)
        {
            return _baseContext.Editions.FirstOrDefault(edition => edition.Id == id);
        }

        public IEnumerable<Edition> GetAll()
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
    }
}
