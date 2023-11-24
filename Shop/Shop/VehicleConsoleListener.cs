using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class VehicleConsoleListener: IDAOListener<Vehicle>
    {
        public void EntityAdded(Vehicle entity)
        {
            Console.WriteLine($"Транспорт доданий: {entity.Name}");
        }

        public void EntityDeleted(int entityId)
        {
            Console.WriteLine($"Транспорт видалений");
        }

        public void EntityUpdated(Vehicle updatedEntity)
        {
            Console.WriteLine($"Транспорт оновлений: {updatedEntity.Name}");
        }
    }
}
