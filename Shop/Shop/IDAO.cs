using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public interface IDAO<T>
    {
        List<T> GetAll();
        T GetByName(string name); 
        T GetById(int id); 
        void Add(T item);
        void Update(T item);
        void Delete(int id);

    }
}
