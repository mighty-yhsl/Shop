using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public interface IDAOListener<T>
    {
        void EntityAdded(T entity);
        void EntityDeleted(int entityId);
        void EntityUpdated(T updatedEntity);
    }
}
 