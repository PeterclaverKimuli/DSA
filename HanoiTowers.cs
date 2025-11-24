using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class HanoiTowers
    {
        private Stack<int> Disks = new Stack<int>();
        private string name;

        public HanoiTowers(string name)
        {
            this.name = name;
        }

        public void Add(int size)
        {
            if (Disks.Count > 0 && Disks.Peek() <= size)
                throw new InvalidOperationException("Cannot place larger disk on top of smaller one.");

            Disks.Push(size);
        }

        private void MoveToTop(HanoiTowers Destination)
        {
            int top = Disks.Pop();
            Destination.Add(top);
            Console.WriteLine($"Move disk {top} from {name} to {Destination.name}");
        }

        public void MoveDisks(int n, HanoiTowers Destination, HanoiTowers Buffer)
        {
            if (n <= 0) return;

            MoveDisks(n - 1, Buffer, Destination);

            MoveToTop(Destination);

            Buffer.MoveDisks(n - 1, Destination, this);
        }
    }
}
