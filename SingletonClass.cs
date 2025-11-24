using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class SingletonClass
    {
        private static SingletonClass? _instance = null;

        protected SingletonClass() {
            // Protected constructor to prevent instantiation from outside
            Console.WriteLine("Singleton instance created.");
        }

        public static SingletonClass GetInstance() {
            if (_instance == null)
                _instance = new SingletonClass();

            return _instance;
        }

        public void DoSomething() { 
            Console.WriteLine("Doing something in the singleton instance.");
        }
    }
}
