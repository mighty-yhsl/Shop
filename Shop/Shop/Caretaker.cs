using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public  class Caretaker
    {
        private List<Memento> _changes = new List<Memento>();

        public void AddChange(Memento change)
        {
            _changes.Add(change);
            Console.WriteLine("Зміни до стану транспортного засобу збережено.");
        }

        public void Undo(Vehicle vehicle)
        {
            if (_changes.Count > 0)
            {
                Memento memento = _changes[_changes.Count - 1];
                _changes.RemoveAt(_changes.Count - 1);

                vehicle.Restore(memento);
                Console.WriteLine("Дія транспортного засобу скасована.");
            }
            else
            {
                Console.WriteLine("Помилка: Немає змінених станів для відміни.");
            }
        }
    }
}
