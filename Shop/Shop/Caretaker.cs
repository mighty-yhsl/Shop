using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public  class Caretaker
    {
        public Stack<Memento> _changes = new Stack<Memento>();

        public void AddChange(Memento change)
        {
            _changes.Push(change);
            Console.WriteLine("Зміни до стану транспортного засобу збережено." + _changes.Count);
        }

        public Memento GetChange() 
        {
             return _changes.Pop();
        }
    }
}
