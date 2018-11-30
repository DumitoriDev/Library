using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    class LanguageRepository : IRepository<Language>
    {
        private static LanguageRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private LanguageRepository() { }
        public static LanguageRepository GetInstance()
        {
            if (instance == null)
                instance = new LanguageRepository();
            return instance;
        }
        public void Add(Language language)
        {
            baseContext.Languages.Add(language);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Languages.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Language Get(int id)
        {
            return baseContext.Languages.FirstOrDefault(language => language.Id == id);
        }

        public IEnumerable<Language> GetAll()
        {
            return baseContext.Languages.ToList();
        }

        public void Update(Language newLanguage)
        {
            Language changeable = Get(newLanguage.Id);
            if (changeable != null)
            {
                changeable.Name = newLanguage.Name;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}
