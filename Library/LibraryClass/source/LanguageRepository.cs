using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class LanguageRepository : IRepository<Language>
    {
        
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
        
       
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

        public Language Get(Func<Language, bool> func)
        {
            return _baseContext.Languages.FirstOrDefault(func);
        }

        public List<Language> GetAll()
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

        public static bool Check(Language language)
        {
            return !string.IsNullOrEmpty(language.Name);
        }

        public bool Check(Func<Language, bool> func)
        {
            return _baseContext.Languages.All(func);
        }

    }
}
