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
            Console.WriteLine($"\nТранспорт доданий: {entity.Name}\n");
        }

        public void EntityDeleted(int entityId)
        {
            Console.WriteLine($"\nТранспорт видалений\n");
        }

        public void EntityUpdated(Vehicle updatedEntity)
        {
            Console.WriteLine($"\nТранспорт оновлений: {updatedEntity.Name}\n");
        }
    }
}
