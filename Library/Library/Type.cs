using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class TypeRepository : IRepository<Type>
    {
        private static TypeRepository instance;
        private DataBaseContext baseContext = DataBaseContext.GetInstance();
        private TypeRepository() { }
        public static TypeRepository GetInstance()
        {
            if (instance == null)
                instance = new TypeRepository();
            return instance;
        }
        public void Add(Type type)
        {
            baseContext.Types.Add(type);
            baseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            baseContext.Types.Remove(Get(id));
            baseContext.SaveChanges();
        }

        public Type Get(int id)
        {
            return baseContext.Types.FirstOrDefault(type => type.Id == id);
        }

        public IEnumerable<Type> GetAll()
        {
            return baseContext.Types.ToList();
        }

        public void Update(Type newType)
        {
            Type changeable = Get(newType.Id);
            if (changeable != null)
            {
                changeable.Name = newType.Name;

                baseContext.Entry(changeable).State = System.Data.Entity.EntityState.Modified;
                baseContext.SaveChanges();
            }
        }
    }
}
