using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class UntilDateRepository:IRepository<UntilDate>
    {
        private readonly DataBaseContext _baseContext = DataBaseContext.GetInstance();
       
        
        public void Add(UntilDate item)
        {
            _baseContext.UntilDate.Add(item);
            _baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _baseContext.UntilDate.Remove(Get(id));
            _baseContext.SaveChanges();
        }

        public UntilDate Get(Func<UntilDate, bool> func)
        {
            return _baseContext.UntilDate.FirstOrDefault(func);
        }
        public UntilDate Get(int id)
        {
            return _baseContext.UntilDate.FirstOrDefault(date => date.Id == id);
        }

        public List<UntilDate> GetAll()
        {
            return _baseContext.UntilDate.ToList();
        }

        public void Update(UntilDate newUntilDate)
        {
            var changeable = Get(newUntilDate.Id);
            if (changeable == null) return;
            changeable.EndTime = newUntilDate.EndTime;
           
            _baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
            _baseContext.SaveChanges();
        }

       
    }
}
