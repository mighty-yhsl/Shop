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
            Console.WriteLine($"Vehicle added: {entity.Name}, Id: {entity.Id}");
        }

        public void EntityDeleted(int entityId)
        {
            Console.WriteLine($"Vehicle deleted, Id: {entityId}");
        }

        public void EntityUpdated(Vehicle updatedEntity)
        {
            Console.WriteLine($"Vehicle updated: {updatedEntity.Name}, Id: {updatedEntity.Id}");
        }
    }
}
