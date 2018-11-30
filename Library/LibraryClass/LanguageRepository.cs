using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class LanguageRepository : IRepository<Language>
    {
        private static LanguageRepository _instance;
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        private LanguageRepository() { }
        public static LanguageRepository GetInstance()
        {
            return _instance ?? (_instance = new LanguageRepository());
        }
        public void Add(Language language)
        {
            _baseContext.Languages.Add(language);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.Languages.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public Language Get(int id)
        {
            return _baseContext.Languages.FirstOrDefault(language => language.Id == id);
        }

        public IEnumerable<Language> GetAll()
        {
            return _baseContext.Languages.ToList();
        }

        public void Update(Language newLanguage)
        {
            var changeable = Get(newLanguage.Id);
            if (changeable == null) return;
            changeable.Name = newLanguage.Name;

            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }
    }
}
