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

            string s;
            s.Substring(0);

            var p = new HashSet<int>();
            var t = p.An
        }

        public void BFS(GNode node)
        {
            var queue = new Queue<GNode>();

            node.Visited = true;
            queue.Enqueue(node);

            while(queue.Count > 0)
            {
                var n = queue.Dequeue();
                visitNode(n);

                foreach(var adj in n.Adjacents)
                {
                    if (!adj.Visited)
                    {
                        adj.Visited = true;
                        queue.Enqueue(adj);
                    }
                }
            }
        }

        public void DFS(GNode node)
        {
            if (node == null)
                return;

            node.Visited = true;
            visitNode(node);

            foreach(var adj in node.Adjacents)
            {
                if (!adj.Visited)
                {
                    DFS(adj);
                }
            }
        }

        public void visitNode(GNode node) {}
    }

    public class GNode
    {
        public string Name;
        public bool Visited;
        public List<GNode> Adjacents;

        public GNode(string name)
        {
            Name = name;
            Visited = false;
            Adjacents = new List<GNode>();
        }
    }
}
