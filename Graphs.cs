using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class GraphNode
    {
        public string Name;
        public bool Visited = false;
        public List<GraphNode> AdjacentNodes = new List<GraphNode>();

        public GraphNode(string name)
        {
            Name = name;
        }

    }

    public class TreeNode
    {
        public int MiddleNodeValue { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode Parent { get; set; }

        public TreeNode(int value)
        {
            MiddleNodeValue = value;
        }
    }

    public class TreeNodeWithParent : TreeNode
    { // This is redundant but included for clarity in the BST Successor problem. I could have just added Parent to TreeNode.
        public TreeNodeWithParent Parent;

        public TreeNodeWithParent(int value) : base(value) { }
    }

    public class TreeNodeWithSize{
        private int size;
        public int MiddleNodeValue;
        public TreeNodeWithSize Left;
        public TreeNodeWithSize Right;

        public TreeNodeWithSize(int value)
        {
            this.size = 1;
            this.MiddleNodeValue = value;
        }

        public void Insert(int value)
        {
            if(value <= MiddleNodeValue)
            {
               if(this.Left == null) this.Left = new TreeNodeWithSize(value);
               else Left.Insert(value);
            }
            else
            {
                if(this.Right == null) this.Right = new TreeNodeWithSize(value);
                else Right.Insert(value);
            }

            size++;
        }

        public TreeNodeWithSize Find(int value)
        {
            if (this == null) return null;

            if (this.MiddleNodeValue == value) return this;
            if(value <  this.MiddleNodeValue) return Left.Find(value);

            return Right.Find(value);
        }

        public TreeNodeWithSize GetRandomNode()
        {
            Random random = new Random();
            int index = random.Next(size);

            return GetRandomNode(index);
        }

        private TreeNodeWithSize GetRandomNode(int index)
        {
            int leftSize = Left != null ? Left.size : 0;

            if(index < leftSize) return Left.GetRandomNode(index);
            if(index == leftSize) return this;

            return Right.GetRandomNode(index - leftSize - 1);
        }

        public TreeNodeWithSize DeleteNode(int value) => DeleteHelper(this, value);

        private TreeNodeWithSize DeleteHelper(TreeNodeWithSize node, int value)
        {
            if (node == null) return null;

            if(value < this.MiddleNodeValue) node.Left = DeleteHelper(node.Left, value);
            else if(value > this.MiddleNodeValue) node.Right = DeleteHelper(node.Right, value);
            else
            {
                // Case 1: No child
                if(node.Left == null && node.Right == null)
                    return null;
                // Case 2: One child
                else if (node.Left == null) 
                    return node.Right;
                else if(node.Right == null)
                    return node.Left;
                // Case 3: Two children
                else
                {
                    TreeNodeWithSize successor = FindMin(node.Right); //We get successor from right because
                    node.MiddleNodeValue = successor.MiddleNodeValue;

                    node.Right = DeleteHelper(node.Right, successor.MiddleNodeValue);
                }

            }

            node.size = 1 + GetSize(node.Left) + GetSize(node.Right); // We recalculate the size parameter for this node 
            return node;
        }

        private TreeNodeWithSize FindMin(TreeNodeWithSize node)
        {
            while(node.Left != null) node = node.Left;

            return node;
        }

        private int GetSize(TreeNodeWithSize node) => node != null ? node.size : 0;
    }

    public class BinaryTree
    {
        public TreeNode root;
    }

    public class Graphs
    {
        public bool BFSRouteBetweenNodes(GraphNode start, GraphNode end) { 
            if(start == end) return true;

            Queue<GraphNode> queue = new Queue<GraphNode>();

            foreach(var node in _GetAllNodes(start)) // _GetAllNodes is a helper function to get all nodes in the graph and reset their Visited property to false.
            {
                node.Visited = false;
            }

            start.Visited = true;
            queue.Enqueue(start);

            while(queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach(var neighbor in current.AdjacentNodes)
                {
                    if (!neighbor.Visited)
                    {
                        if (neighbor == end) return true;

                        neighbor.Visited = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return false;
        }

        public bool DFSRouteBetweenNode(GraphNode start, GraphNode end)
        {
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            return DFS(start, end, visited);
        }

        public bool DFS(GraphNode current, GraphNode target, HashSet<GraphNode> visited)
        {
            if (current == null) return false;

            if (current == target) return true;

            visited.Add(current);

            foreach (var neighbor in current.AdjacentNodes)
            {
                if (!visited.Contains(neighbor))
                {
                    if (DFS(neighbor, target, visited)) return true;
                }
            }

            return false;
        }

        private HashSet<GraphNode> _GetAllNodes(GraphNode start) { 
            HashSet<GraphNode> nodes = new HashSet<GraphNode>();
            Stack<GraphNode> stack = new Stack<GraphNode>();

            stack.Push(start);

            while (stack.Count > 0) {
                var currentNode = stack.Pop();

                if(!nodes.Contains(currentNode)) { 
                    nodes.Add(currentNode);

                    foreach(var neighbor in currentNode.AdjacentNodes)
                    {
                        stack.Push(neighbor);
                    }
                }
            }

            return nodes;
        }
    }

    public class MinimalBST
    {
        public TreeNode CreateMinimalBST(int[] intArray)
        {
            return CreateMinimalBST(intArray, 0, intArray.Length - 1);
        }

        public TreeNode CreateMinimalBST(int[] intArray, int start, int end)
        {
            if (end < start) return null;

            int mid = (start + end) / 2;

            var node = new TreeNode(intArray[mid]);
            node.Left = CreateMinimalBST(intArray, start, mid - 1);
            node.Right = CreateMinimalBST(intArray, mid + 1, end);

            return node;
        }
    }

    public class DLinkedLists
    {
        public List<LinkedList<TreeNode>> BFSDLists(TreeNode root) {
            //Time Complexity: O(N) — where N is the number of nodes(we visit each node once).
            //Space Complexity: O(N) — for storing the lists and queue.

            var list = new List<LinkedList<TreeNode>>();
            if (root == null) return list;

            var queue = new Queue<TreeNode>();

            queue.Enqueue(root);

            while (queue.Count > 0) { 
                var tempList = new LinkedList<TreeNode>();

                var thisLevelCount = queue.Count;

                for (int i = 0; i < thisLevelCount; i++)
                {
                    var current = queue.Dequeue();
                    tempList.AddLast(current);

                    if (current.Left != null)
                    {
                        queue.Enqueue(current.Left);
                    }

                    if (current.Right != null)
                    {
                        queue.Enqueue(current.Right);
                    }
                }

                list.Add(tempList);
            }

            return list;
        }

        public List<LinkedList<TreeNode>> DFSDLists(TreeNode root)
        {
            var result = new List<LinkedList<TreeNode>>();

            DFS(root, 0, result);

            return result;
        }

        private void DFS(TreeNode node, int depth, List<LinkedList<TreeNode>> list)
        {
            if(node == null) return;

            // First time we've reached this depth
            if (list.Count == depth) list.Add(new LinkedList<TreeNode>());

            list[depth].AddLast(node);

            DFS(node.Left, depth + 1, list);
            DFS(node.Right, depth + 1, list);
        }
    }

    public class CheckBalanced
    {
        public bool isBalanced(TreeNode root)
        {
            var nodeLength = NodeLength(root);

            return nodeLength != -1;
        }

        private int NodeLength(TreeNode node)
        {
            if (node == null) return 0;

            var leftLength = NodeLength(node.Left);
            if (leftLength == -1) return -1; // Left subtree not balanced

            var rightLength = NodeLength(node.Right);
            if (rightLength == -1) return -1; // Right subtree not balanced

            if (Math.Abs(leftLength - rightLength) > 1) return -1; // Current node not balanced

            return Math.Max(leftLength, rightLength) + 1; // We add 1 for the current node
        }
    }

    public class ValidateBST
    {
        public bool isBinarySearchTree(TreeNode root)
        {
            return isBinarySearchTreeHelper(root, null, null);
        }

        private bool isBinarySearchTreeHelper(TreeNode node, int? min, int? max)
        {
            if (node == null) return true;

            // Node value must be > min (for right subtrees) and <= max (for left subtrees)
            if ((min != null && node.MiddleNodeValue <= min /*right subtree check*/) 
                || (max != null && node.MiddleNodeValue > max /*left subtree check*/)) 
                return false; 

            return isBinarySearchTreeHelper(node.Left, min, node.MiddleNodeValue) && isBinarySearchTreeHelper(node.Right, node.MiddleNodeValue, max); // we && because both sides need to be true to return true
        }
    }

    public class BSTSuccessor
    {
        public TreeNode GetNodeSuccessor(TreeNodeWithParent node) {
            if (node == null) 
                return null;

            // Case 1: Right subtree exists
            if (node.Right != null) 
                return GetLeftMostChild(node.Right); // If node has a right subtree, the successor is the leftmost node in that subtree because it's the smallest value greater than the current node in BST.

            // Case 2: No right subtree - go up until node is left child
            var current = node;
            var parent = node.Parent;

            // If node has no right subtree, go up the tree until we find a node that is the left child of its parent.
            // The parent of that node is the successor. This is because the parent is the next larger value after the current node in a BST.
            while (parent != null && current == parent.Right) 
            {
                current = parent;
                parent = parent.Parent;
            }

            return parent;
        }

        private TreeNode GetLeftMostChild(TreeNode node)
        {
            while (node.Left != null)
            {
                node = node.Left; 
            }

            return node;
        }
    }

    public class TopologicalBuildOrder
    {
        public List<string> FindBuildOrder(List<string> projects, List<(string, string)> dependencies)
        {
            var graph = new Dictionary<string, List<string>>();
            var inDegree = new Dictionary<string, int>();

            // Initialize graph and in-degree(prerequisites) count.
            // In-degree is the number of incoming edges to a node, that is how many prerequisites a project has.
            //graph represents the adjacency list of the directed graph, that is for each project, it lists the projects that depend on it.
            foreach (var project in projects)
            {
                graph[project] = new List<string>();
                inDegree[project] = 0;
            }

            // Build the graph and fill in-degrees
            foreach (var (before, after) in dependencies) //after depends on before
            {
                graph[before].Add(after);
                inDegree[after]++;
            }

            // Find all nodes with 0 in-degree to start
            var queue = new Queue<string>();

            foreach(var project in projects)
            {
                if (inDegree[project] == 0)
                    queue.Enqueue(project);
            }

            var buildOrder = new List<string>();

            // Topological sort
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                buildOrder.Add(current);

                foreach(var neighbor in graph[current])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0) queue.Enqueue(neighbor);
                }
            }

            // If not all projects are in the build order, a cycle exists
            if(buildOrder.Count != projects.Count)
            {
                throw new InvalidOperationException("ERROR: No valid build order (circular dependency).");
            }

            return buildOrder;
        }
    }
}
