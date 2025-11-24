using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public abstract class FactoryClass
    {
        // Step 1: Create a base class
        public abstract void ShortIntro();
    }

    public class  FirstInstance : FactoryClass
    {
        public override void ShortIntro()
        {
            Console.WriteLine("This is the first instance of FactoryClass.");
        }
    }

    public class SecondInstance : FactoryClass {
        public override void ShortIntro()
        {
            Console.WriteLine("This is the second instance of FactoryClass.");
        }
    }

    // Step 3: Enum for types
    public enum FactoryType {
        FirstInstance,
        SecondInstance
    }

    // Step 4: Factory method
    public static class FactoryMethod {
        public static FactoryClass CreateFactoryClassInstance(FactoryType type) {
            if (type is FactoryType.FirstInstance) {
                return new FirstInstance();
            }
            else if (type is FactoryType.SecondInstance)
            {
                return new SecondInstance();
            }

            return null; // or throw an exception if type is invalid
        }
    }
}
