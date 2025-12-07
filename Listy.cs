using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class Listy
    {
        private int[] data;

        public Listy(int[] array)
        {
            data = array;
        }

        public int ElementAt(int i)
        {
            if (i < 0 || i >= data.Length)
                return -1;

            return data[i];
        }
    }
}
