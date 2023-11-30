using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class Observer : IObserver
    {
        public void Update(string message) 
        { 
            Console.WriteLine(message);
        }
    }
}
