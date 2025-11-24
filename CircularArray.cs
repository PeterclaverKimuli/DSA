using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class CircularArray<T> : IEnumerable<T>
    {
        private T[] items;
        private int head = 0;

        public int Length => items.Length;

        public CircularArray(int size)
        {
            items = new T[size];
        }

        public T this[int index] {
            get {
                if(index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();

                return items[ConvertIndex(index)];
            }

            set { 
                if(index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();

                items[ConvertIndex(index)] = value;
            }
        }

        private int ConvertIndex(int index)
        {
            return (index + head) % Length;
        }

        public void Rotate(int shift)
        {
            head = (head + shift) % Length;

            if (head < 0)
                head += Length; // wrap-around for negative shifts
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(int i = 0; i < Length; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() { 
            return GetEnumerator();
        }
    }
}
