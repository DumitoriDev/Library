using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Edition
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    class EditionRepository : IRepository<Edition>
    {
        private static EditionRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private EditionRepository() { }
        public static EditionRepository GetInstance()
        {
            if (instance == null)
                instance = new EditionRepository();
            return instance;
        }
        public void Add(Edition edition)
        {
            baseContext.Editions.Add(edition);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Editions.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Edition Get(int id)
        {
            return baseContext.Editions.FirstOrDefault(edition => edition.Id == id);
        }

        public IEnumerable<Edition> GetAll()
        {
            return baseContext.Editions.ToList();
        }

        public void Update(Edition newEdition)
        {
            Edition changeable = Get(newEdition.Id);
            if (changeable != null)
            {
                changeable.Name = newEdition.Name;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}
