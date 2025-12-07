// See https://aka.ms/new-console-template for more information
using KaratExercises;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;

Console.WriteLine(stringCompression("aaa"));

// number >> 1 is equivalent to number / 2 for positive numbers
// number << 1 is equivalent to number * 2 for positive numbers
// number & 1 is equivalent to number % 2 for positive numbers
// number ^ 1 flips the last bit (0 to 1 or 1 to 0)
// number & (number - 1) clears the lowest set bit (turns the rightmost 1 to 0)
// number >> k is equivalent to number / (2^k) for positive numbers
// number << k is equivalent to number * (2^k) for positive numbers

/*
 * int[,] — Rectangular Array : Every row has the same number of columns.
 *  Example: int[,] grid = new int[2, 3]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

 * int[][] — Jagged Array (This is an array of arrays.): Each row is its own separate array, which means rows can have different lengths.
 * Example:
       int[][] jagged = new int[2][];
       jagged[0] = new int[] { 1, 2, 3 };
       jagged[1] = new int[] { 4, 5 };
       jagged[2] = new int[] {3,5,6}

       int[][] jag = new int[2][];
       jag[0] = new int[]{3,5,3};
 */


#region 1 strings and arrays
#region stringRotation
static bool isRotation(string s1, string s2)
{
    var len = s1.Length;

    /* Check that s1 and s2 are equal length and not empty*/
    if (len == s2.Length && len > 0)
    {
        /* Concatenate s1 and s1 within new buffer */
        var s1s1 = s1 + s1; 

        return isSubstring(s2, s1s1);
    }

    return false;
}

//This method is not implemented. It is just cited in the algorithm above.
static bool isSubstring(string s2, string s1s1)
{
    return true;
}
#endregion

#region zeroTheMatrix
static void setZero(int[,] matrix)
{
    int rowLen = matrix.GetLength(0);
    int colLen = matrix.GetLength(1);

    var firstRowHasZero = false;
    var firstColHasZero = false;

    //Check if first row has zero
    for (int i = 0; i < colLen; i++)
    {
        if (matrix[0, i] == 0)
        {
            firstRowHasZero = true;
            break;
        }
    }

    //Check if first column has zero
    for (int i = 0; i < rowLen; i++)
    {
        if (matrix[i, 0] == 0)
        {
            firstColHasZero = true;
            break;
        }
    }

    //iterate through matrix making first row and first col zero if an index is zero
    for (int i = 1; i < rowLen; i++)
    {
        for (int j = 1; j < colLen; j++)
        {
            if (matrix[i, j] == 0)
            {
                matrix[0, j] = 0;
                matrix[i, 0] = 0;
            }
        }
    }

    //Iterate through matrix again and make a respective index zero if the correponding first row or col index is zero
    for (int i = 1; i < rowLen; i++)
    {
        for (int j = 1; j < colLen; j++)
        {
            if (matrix[0, j] == 0 || matrix[i, 0] == 0)
            {
                matrix[i, j] = 0;
            }
        }
    }

    //make first row zero if the firstRowZero indicator is true
    if (firstRowHasZero)
    {
        for (int i = 0; i < colLen; i++)
        {
            matrix[0, i] = 0;
        }
    }

    //make first col zero if the firstColZero indicator is true
    if (firstColHasZero)
    {
        for (int i = 0; i < rowLen; i++)
        {
            matrix[i, 0] = 0;
        }
    }
}
#endregion

#region rotateMatrix
static void rotateMatrix(int[,] matrix)
{
    int n = matrix.GetLength(0);

    for (int layer = 0; layer < n / 2; layer++)
    {
        int first = layer;
        int last = n - 1 - layer;

        for (int i = first; i < last; i++)
        {
            // Top: matrix[first, i]
            // Right: matrix[i, last]
            // Bottom: matrix[last, last - offset]
            // Left: matrix[last - offset, first]

            int offset = i - first;

            //save top 
            int top = matrix[first, i];

            //left -> top
            matrix[first, i] = matrix[last - offset, first];

            //bottom -> left
            matrix[last - offset, first] = matrix[last, last - offset];

            //right -> bottom
            matrix[last, last - offset] = matrix[i, last];

            //top -> right
            matrix[i, last] = top;
        }
    }
}
#endregion

#region string compression
static string stringCompression(string str)
{
    if (string.IsNullOrEmpty(str))
        return str;

    var compressed = new StringBuilder();
    int countConsecutive = 0;

    for (int i = 0; i < str.Length; i++)
    {
        countConsecutive++;

        // If next char is different or we’re at the end
        if (i + 1 >= str.Length || str[i] != str[i + 1])
        {
            compressed.Append(str[i]);
            compressed.Append(countConsecutive);
            countConsecutive = 0;
        }
    }

    string result = compressed.ToString();
    return result.Length < str.Length ? result : str;
}
#endregion

#region oneAway
static bool oneAway(string str, string strComp)
{
    if (Math.Abs(str.Length - strComp.Length) > 1) return false;

    if (str.Length == strComp.Length)
    {
        return oneEditReplace(str, strComp);
    }
    else if (str.Length > strComp.Length)
    {
        return oneEditInsert(strComp, str);
    }
    else if (str.Length < strComp.Length)
    {
        return oneEditInsert(str, strComp);
    }

    return false;
}

static bool oneEditReplace(string str, string strComp)
{
    bool foundDiff = false;

    for (int i = 0; i < strComp.Length; i++)
    {
        if (str[i] != strComp[i])
        {
            if (foundDiff) return false;

            foundDiff = true;
        }
    }

    return true;
}

static bool oneEditInsert(string shorter, string longer)
{
    int index1 = 0, index2 = 0;
    bool foundDiff = false;

    while (index1 < shorter.Length && index2 < longer.Length)
    {
        if (shorter[index1] != longer[index2])
        {
            if (foundDiff) return false;

            foundDiff = true;
            index2++;
        }
        else
        {
            index1++;
            index2++;
        }
    }

    return true;
}
#endregion

#region palindromePermutation
static bool palindromePermutation(string str)
{
    int countOfOdds = 0;
    str = str.ToLower();

    var dict = new Dictionary<char, int>();

    foreach (char c in str)
    {
        if (!char.IsLetter(c)) continue;

        if (dict.ContainsKey(c))
        {
            dict[c]++;
        }
        else
            dict[c] = 1;
    }

    foreach (int num in dict.Values)
    {
        if (num % 2 > 0)
        {
            countOfOdds++;
        }

        if (countOfOdds > 1) return false;
    }

    return true;
}
#endregion

#region urlify
static string urlify(string str, int trueStrLen)
{
    if (String.IsNullOrEmpty(str)) return str;

    var chars = new StringBuilder();

    for (int i = 0; i < trueStrLen; i++)
    {
        if (str[i] == ' ')
        {
            chars.Append("%20");
        }
        else
        {
            chars.Append(str[i]);
        }
    }

    return chars.ToString();
}

void urlifyInPlace(char[] str, int trueStrLen)
{
    int spaceCount = 0;

    for(int i = 0; i < trueStrLen; i++)
    {
        if (str[i] == ' ') spaceCount++;
    }

    int index = trueStrLen + (spaceCount * 2) - 1; //Each space is replaced by 3 chars, so we need 2 extra spaces per space

    // Fill array backwards
    for (int i = trueStrLen - 1; i >= 0; i--)
    {
        if (str[i] == ' ')
        {
            str[index--] = '0';
            str[index--] = '2';
            str[index--] = '%';
        }
        else
        {
            str[index--] = str[i];
        }
    }
}
#endregion

#region isPermutation
static bool isPermutation(string str, string strToCheck)
{
    if (String.IsNullOrEmpty(strToCheck) || String.IsNullOrEmpty(str)) return false;
    if (strToCheck.Length != str.Length) return false;

    var dict = new Dictionary<char, int>();

    foreach (char c in str)
    {
        if (dict.ContainsKey(c))
            dict[c] = dict[c] + 1;

        dict[c] = 1;
    }

    foreach (char c in strToCheck)
    {
        if (!dict.ContainsKey(c))
        {
            return false;
        }
        else dict[c] = dict[c] - 1;

        if (dict[c] < 0) return false;
    }

    return true;
}

bool isPermutationASCII(string str, string strToCheck)
{
    if (str.Length != strToCheck.Length) return false;

    int[] charset = new int[128]; //Assuming ASCII

    foreach(char c in str) //II count number of each char in s.
    {
        charset[c]++;
    }

    for(int i = 0; i < strToCheck.Length; i++)
    {
        charset[str[i]]--;

        if (charset[str[i]] < 0) return false;
    }

    return true;
}
#endregion

#region uniqueCharacters
static bool uniqueCharacters(string str)
{
    var dict = new Dictionary<char, int>();

    if (String.IsNullOrEmpty(str))
        return false;

    foreach (char c in str)
    {
        if (dict.ContainsKey(c))
            return false;

        dict[c] = 1;
    }

    return true;
}

bool anotherUniqueCharacters(string str)
{
    if(str.Length > 128) return false; //Assuming ASCII

    if (String.IsNullOrEmpty(str))
        return false;

    bool[] charset = new bool[128];

    for(int i = 0; i < str.Length; i++)
    {
        int val = str[i];

        if (charset[val]) return false;

        charset[val] = true;
    }

    return true;
}
#endregion


#endregion

#region 2 linked lists

#region remove dups from unsorted linked list
//with buffer
static void removeDupsBuffer(Node head){
    Node prev = null;
    HashSet<int> seen = new HashSet<int>();

    while (head != null) {
        if (seen.Contains(head.Data)) {
            prev.Next = head.Next;
        }
        else
        {
            seen.Add(head.Data);
            prev = head;
        }

        head = head.Next;
    }
}

//without buffer
static void removeDupsNoBuffer(Node head)
{
    Node current = head;

    while (current != null)
    {
        Node runner = current;
        while (runner.Next != null) {
            if (runner.Next.Data == current.Data) { 
                runner.Next = runner.Next.Next;
            }
            else
            {
                runner = runner.Next;
            }
        }

        current = current.Next;
    }
}

#endregion

#region kth to last element
static Node kthToLast(Node head, int k)
{
    if (head == null || k < 1) return null;

    Node p1 = head;
    Node p2 = head;

    for (int i = 0; i < k; i++) {
        if (p1 == null) return null; // k is larger than the list length

        p1 = p1.Next;
    }

    while (p1 != null) { 
        p1 = p1.Next;
        p2 = p2.Next;
    }

    return p2;
}

//This algorithm takes O(n) time and 0(1) space. 
#endregion

#region delete middle node, not necessarily the exact middle one
static void deleteMiddleNode(Node middle)
{
    if(middle == null || middle.Next == null) return;

    middle.Data = middle.Next.Data;
    middle.Next = middle.Next.Next;
}
#endregion

#region Partion linked list
static Node Partition(Node linkedList, int x)
{
    //Create LinkedList for elements less than x
    Node beforeListStart = null;
    Node beforeListEnd = null;

    //Create LinkedList for elements greater than x
    Node afterListStart = null;
    Node afterListEnd = null;

    Node current = linkedList;

    while (current != null) { 
        Node next = current.Next;
        current.Next = null;

        if (current.Data < x)
        {
            if (beforeListStart == null)
            {
                //Here we are starting the before list
                beforeListStart = current;
                beforeListEnd = beforeListStart;
            }
            else
            {
                beforeListEnd.Next = current;
                beforeListEnd = current;
            }
        }
        else
        {
            if (afterListStart == null)
            {
                //Here we are starting the after list
                afterListStart = current;
                afterListEnd = afterListStart;
            }
            else
            {
                afterListEnd.Next = current;
                afterListEnd = current;
            }
        }

        current = next;
    }

    if (beforeListStart == null)
        return afterListStart;

    beforeListEnd.Next = afterListStart;

    return beforeListStart;
}
#endregion

#region add reveresed numbers in linked list
static Node addLinkedListReversedNums(Node firstList, Node secondList)
{
    Node dummyHead = new Node(0);
    Node sumList = dummyHead;
    int carry = 0;

    while (firstList != null || secondList != null || carry != 0) { 
        var firstNum = firstList != null ? firstList.Data : 0;
        var secondNum = secondList != null ? secondList.Data : 0;

        var sum = firstNum + secondNum + carry;

        carry = sum / 10;
        sumList.Next = new Node(sum % 10);
        sumList = sumList.Next;

        if(firstList != null) 
            firstList = firstList.Next;

        if(secondList != null) 
            secondList = secondList.Next;
    }

    return dummyHead.Next;
}
#endregion

#region add normal numbers in linked list
static Node addLinkedListNums(Node firstList, Node secondList)
{
    return LinkedListAdder.ListsAdder(firstList, secondList);
}
#endregion

#region is linked list a palindrome
static bool isLinkedListPalindrome(Node list)
{
    if (list == null || list.Next == null) return true;

    //find middle of the list
    var slowPointer = list;
    var fastPointer = list;

    while (fastPointer != null && fastPointer.Next != null) {
        slowPointer = slowPointer.Next;
        fastPointer = fastPointer.Next.Next;
    }

    //Has odd number of elements so we skip the middle one
    if (fastPointer != null) slowPointer = slowPointer.Next;

    Node reversedHalf = reverseList(slowPointer);

    var firstHalf = list;
    var secondHalf = reversedHalf;

    while(secondHalf != null)
    {
        if (firstHalf.Data != secondHalf.Data) return false;

        firstHalf = firstHalf.Next;
        secondHalf = secondHalf.Next;
    }

    return true;
}

static Node reverseList(Node list)
{
    Node previous = null;

    while (list != null) { 
        Node nextNode = list.Next;

        list.Next = previous;
        previous = list;

        list = nextNode;
    }

    return previous;
}
#endregion

#region intersection of linked Lists
static LengthAndTail getLengthAndTail(Node list)
{
    int length = 0;

    while (list != null)
    {
        length++;
        if(list.Next == null)
        {
            return new LengthAndTail(length, list);
        }

        list = list.Next;
    }

    return null;
}

static Node getKthNode(Node node, int k)
{
    Node current = node;

    while(k > 0 && current != null)
    {
        current = current.Next;
        k--;    
    }

    return current;
}

static Node findIntersection(Node firstList, Node secondList)
{
    if (firstList == null || secondList == null) return null;

    LengthAndTail first = getLengthAndTail(firstList);
    LengthAndTail second = getLengthAndTail(secondList);

    // If different tail nodes, they do not intersect
    if (first.Tail != second.Tail) return null;

    // Set pointers to the start of each list
    Node shorter = first.Length < second.Length ? firstList : secondList;
    Node longer = first.Length < second.Length ? secondList : firstList;

    // Advance the pointer for the longer list
    longer = getKthNode(longer, Math.Abs(first.Length - second.Length));

    // Move both pointers until they collide
    while (longer != shorter)
    {
        longer = longer.Next;
        shorter = shorter.Next;
    }

    return shorter;
}

//This algorithm takes O(A + B) time, where A and Bare the lengths of the two linked lists. It takes O( 1) additional space. 
#endregion

#region Get start of loop in linked list
static Node getLoopStart(Node list)
{
    Node slow = list;
    Node fast = list;

    // Step 1: Detect meeting point
    while (fast != null && fast.Next != null)
    {
        slow = slow.Next; 
        fast = fast.Next.Next;

        if(fast == slow) break;
    }

    // No loop
    if (fast == null || fast.Next == null) return null;

    //Move slow to Head. Keep fast at Meeting Point.Each are k steps from the Loop Start.
    //If they move at the same pace, they must meet at Loop Start.
    slow = list;

    while(slow != fast)
    {
        slow = slow.Next;
        fast = fast.Next;
    }

    // Both now point to the start of the loop.
    return fast;
}
#endregion

#endregion

#region 3 Stacks and Queues

#region one array three stacks
var answer = new OneArrayThreeStacks(3);
#endregion

#region stack min
var result = new MinStack();
#endregion

#region stack of plates
var stackOfPlates = new SetOfStacks(5);
#endregion

#region Queue using two stacks
var queueUsingStacks = new MyQueueUsingStacks<int>();
#endregion

#region Sort Stack
static void sortStack(Stack<int> stack)
{
    var tempStack = new Stack<int>();

    while(stack.Count > 0)
    {
        int temp = stack.Pop();

        while(tempStack.Count > 0 && tempStack.Peek() > temp)
        {
            stack.Push(tempStack.Pop());
        }

        tempStack.Push(temp);
    }

    while (tempStack.Count > 0) { 
        stack.Push(tempStack.Pop());
    }

}
#endregion

#region Animal Shelter queue
var animalShelter = new AnimalShelterQueue();
#endregion

#endregion

#region 4 Trees and Graphs

#region In order traversal
// In order traversal means to "visit" (often, print) the left branch, then the current node, and finally, the right branch. 
// When performed on a binary search tree, it visits the nodes in ascending order (hence the name "in-order"). 
void inOrderTraversal(TreeNode node)
{
    if(node != null)
    {
        inOrderTraversal(node.Left);
        visit(node);
        inOrderTraversal(node.Right);
    }
}

void visit(TreeNode node) { }

#endregion

#region Pre-order traversal
// Pre-order traversal visits the current node before its child nodes (hence the name "pre-order"). 
// In a pre-order traversal, the root is always the first node visited. 

void preOrderTraversal(TreeNode node)
{
    if(node != null)
    {
        visit(node);
        preOrderTraversal(node.Left);
        preOrderTraversal(node.Right);
    }
}

#endregion

#region Post-order traversal
// Post-order traversal visits the current node after its child nodes (hence the name "post-order"). 
// In a post-order traversal, the root is always the last node visited.

void postOrderTraversal(TreeNode node)
{
    if(node != null)
    {
        preOrderTraversal(node.Left);
        preOrderTraversal(node.Right);
        visit(node);
    }
}

#endregion

#region DFS - Depth First Search

void search(GraphNode root)
{
    if (root == null)
        return;

    visitNode(root);
    root.Visited = true;

    foreach(var node in root.AdjacentNodes)
    {
        if (!node.Visited)
        {
            search(node);
        }
    } 
}

void visitNode(GraphNode node) { }

#endregion

#region BFS - Breadth First Search

void BFSearch(GraphNode node)
{
    var queue = new Queue<GraphNode>();
    node.Visited = true;
    queue.Enqueue(node);

    while(queue.Count > 0)
    {
        var n = queue.Dequeue();
        visitNode(n);

        foreach(var adjacent in n.AdjacentNodes)
        {
            if (!adjacent.Visited)
            {
                adjacent.Visited = true;
                queue.Enqueue(adjacent);
            }
        }
    }
}

#endregion

#region Route between Nodes LeetCode: #1971

var routeBetweenRoutes = new Graphs();

#endregion

#region Minimal Tree LeetCode: #108
var minimalTree = new MinimalBST();
#endregion

#region List of Depths 
var dLinkedLists = new DLinkedLists();
#endregion

#region Check Balanced LeetCode: #110
var checkBalance = new CheckBalanced();
#endregion

#region Validate BST LeetCode: #98
var validateBST = new ValidateBST();

//The time complexity for this solution is O(N), where N is the number of nodes in the tree since we must touch all N nodes.
/* Due to the use of recursion, the space complexity is O ( log N) on a balanced tree. 
 * There are up to O (log N) recursive calls on the stack since we may recurse up to the depth of the tree.
 */

#endregion

#region In-Order BT Successor LeetCode: #285.
var bstSuccessor = new BSTSuccessor();
#endregion

#region Build Order  LeetCode 210. Course Schedule II (Medium) **Revisit this!!!**
var buildOrder = new TopologicalBuildOrder();
#endregion

#region First Common Ancestor
static TreeNode FindCommonAncestor(TreeNode root, TreeNode node1, TreeNode node2)
{
    if (root == null || root == node1 || root == node2) // Stop if root is equal to either nodes and return that root
        return root;

    var left = FindCommonAncestor(root.Left, node1, node2); // Search left subtree
    var right = FindCommonAncestor(root.Right, node1, node2); // Search right subtree

    if (left != null && right != null) // If both left and right are non-null, we found both nodes and the root at this level is the common ancestor
        return root;

    return left != null ? left : right; // Either one node was found on left or right, or neither was found
}
#endregion

#region BST Sequences **Revisit this!!!**
static List<List<int>> getBstSequences(TreeNode root)
{
    var result = new List<List<int>>();

    if (root == null)
    {
        result.Add(new List<int>()); // Base case: return list with empty list
        return result;
    }

    var prefix = new List<int>() { root.MiddleNodeValue };

    var leftSequence = getBstSequences(root.Left);
    var rightSequence = getBstSequences(root.Right);

    foreach(var left in leftSequence)
    {
        foreach(var right in rightSequence)
        {
            var weaved = new List<List<int>>();

            WeaveList(left, right, weaved, prefix);

            result.AddRange(weaved);
        }
    }

    return result;
}

static void WeaveList(List<int> first, List<int> second, List<List<int>> results, List<int> prefix)
{
    if (first.Count == 0 || second.Count == 0) {
        var result = new List<int>(prefix);
        result.AddRange(first);
        result.AddRange(second);
        results.Add(result);
        return;
    }

    // Recurse with head of first
    int headFirst = first[0];
    first.RemoveAt(0);
    prefix.Add(headFirst);
    WeaveList(first, second, results, prefix);
    prefix.RemoveAt(prefix.Count - 1);
    first.Insert(0, headFirst); //we backtrack because to reuse the original list in next level of recursion

    // Recurse with head of second
    int headSecond = second[0];
    second.RemoveAt(0);
    prefix.Add(headSecond);
    WeaveList(first, second, results, prefix);
    prefix.RemoveAt(prefix.Count - 1);
    second.Insert(0, headSecond);
}
#endregion

#region Check Subtree
static bool checkIfSubtree(TreeNode T1, TreeNode T2)
{
    string t1String = GetPreOrderString(T1);
    string t2String = GetPreOrderString(T2);

    return t1String.Contains(t2String);
}

static string GetPreOrderString(TreeNode node)
{
    var sb = new StringBuilder();
    BuildPreorder(node, sb);

    return sb.ToString();
}

static void BuildPreorder(TreeNode node, StringBuilder sb) // This follows the pre-order traversal approach (Root -> left -> Right)
{
    if (node == null) { 
        sb.Append("X "); 
        return;
    }

    sb.Append(node.MiddleNodeValue + " ");
    BuildPreorder(node.Left, sb); // Goes all the way down to the leftmost node before it can begin going right 
    BuildPreorder(node.Right, sb);
}

//Time: O(n + m)
//Space: O(n + m) to store both strings
#endregion

#region Random Node ** Revisit this!!! **
var randomNode = new TreeNodeWithSize(10);
#endregion

#region Path with Sum (Prefix sums)
static int getNumberOfPaths(TreeNode root, int targetSum)
{
    var prefixSums = new Dictionary<int, int>();
    prefixSums[0] = 1; // Base case for root

    return countPaths(root, 0, targetSum, prefixSums);
}

static int countPaths(TreeNode node, int currentSum, int targetSum, Dictionary<int, int> prefixSums)
{
    if(node == null) return 0;

    currentSum += node.MiddleNodeValue;
    int count = 0;

    // Check if there’s a prefix path we can subtract to reach target; Check if we have seen a total earlier that makes a path = target
    if (prefixSums.ContainsKey(currentSum - targetSum))
        count += prefixSums[currentSum - targetSum];

    // Add currentSum to map; Save this running sum so children can use it
    if (!prefixSums.ContainsKey(currentSum)) prefixSums[currentSum] = 0;
    prefixSums[currentSum]++;

    //Explore children 
    count += countPaths(node.Left, currentSum, targetSum, prefixSums);
    count += countPaths(node.Right, currentSum, targetSum, prefixSums);

    // Backtrack (remove this runningSum when leaving this node)
    prefixSums[currentSum]--;

    return count;
}
#endregion

#endregion

#region 5. Bit Manipulation

#region Common bit tasks

bool getBit(int num, int i)
{
    var mask = 1 << i; // Create a mask by shifting 1, i times to the left
    var newNum = num & mask; // Use bitwise AND to isolate the bit at position i

    return newNum != 0; // If the result is not zero, the bit at position i is 1; otherwise, it's 0
}

int setBit(int num, int i)
{
    var mask = 1 << i; // Create a mask by shifting 1, i times to the left
    return num | mask; // Use bitwise OR to set the bit at position i to 1
}

int clearBit(int num, int i)
{
    var mask = ~(1 << i); //Create a mask by shifting 1, i times to the left and then inverting all bits

    return num & mask; // Use bitwise AND with the negated mask to set the bit at position i to 0 and leave all other bits unchanged
}

int clearBitsFromMSBThroughI(int num, int i)
{
    var mask = (1 << i) - 1; // Create a mask with 1s from bit 0 to i-1 and 0s from bit i to MSB

    return num & mask; // Use bitwise AND to clear bits from MSB through i
}

int clearBitsFromIThrough0(int num, int i)
{
    var mask = (-1 << (i + 1)); // Create a mask with 0s from bit 0 to i and 1s from bit i+1 to MSB

    return num & mask; // Use bitwise AND to clear bits from i through 0
}

int updateBit(int num, int i, bool bitIs1)
{
    var value = bitIs1 ? 1 : 0; // Determine the value to set (1 or 0)
    var mask = ~(1 << i); // Create a mask by shifting 1, i times to the left and then inverting all bits 

    return (num & mask) | (value << i); // Clear the bit at position i and then set it to the desired value using bitwise OR
}
#endregion

#region Insertion
static int InsertBits(int N, int M, int i, int j)
{
    int allOnes = ~0; // allOnes = 11111111111111111111111111111111

    var left = allOnes << (i + j); // Create left part of mask left = 11111111111111111111111110000000
    var right = (1 << i) - 1; // Create right part of mask right = 00000000000000000000000000000011

    var mask = left | right; // combine the parts mask = 11111111111111111111111110000011

    var n_cleared = N & mask; // Apply mask to N
    //  N        = 0000010000000000 (binary of 1024)
    //  mask     = 1111111111000011
    //  result   = 0000010000000000 & 1111111111000011
    //            = 0000010000000000

    var m_shifted = M << i; // Shift M to align with position i
    // M        =       0000010011
    // M << 2   =       0001001100 (which is 76 in decimal)

    return n_cleared | m_shifted; // Merge M into N
    // n_cleared = 10000000000 (binary)
    // m_shifted = 0001001100

    // Result =    10000000000
    //           | 0001001100
    //           = 10001001100 (binary)
}
#endregion

#region Binary to String
/* Key Idea: How to convert a fraction to binary
0.625 * 2 = 1.25     → take 1     → .1
0.25  * 2 = 0.5      → take 0     → .10
0.5   * 2 = 1.0      → take 1     → .101
 */

static string BinaryToString(double num)
{
    if (num <= 0 || num >= 1)
        return "ERROR";

    var sb = new StringBuilder();
    sb.Append(".");

    while(num > 0)
    {
        if (sb.Length > 32) 
            return "ERROR";

        num *= 2;

        if (num >= 1)
        {
            sb.Append("1");
            num -= 1;
        }
        else sb.Append("0");
    }

    return sb.ToString();
}
#endregion

#region Flip Bit to win
int FlipBitToWin(int num)
{
    if (~num == 0) return 32; // All bits are 1s

    var previousLength = 0;
    var currentLength = 0;
    var maxLenghth = 1; // We can always have at least one 1

    while (num != 0)
    {
        if ((num & 1) == 1) //1 in binary is 0001 so this is equivalent to 1101 & 0001 = 0001 → 1, if num = 1101
        {
            currentLength++;
        }
        else
        {
            // If next bit is 0, reset previousLength to 0. Otherwise, keep currentLength.
            previousLength = (num & 2) == 0 ? 0 : currentLength; // Check if next bit (second to last bit) is 1. 2 in binary is 0010 so this is equivalent to 1101 & 0010 = 0000 → 0, if num = 1101
            currentLength = 0;
        }

        maxLenghth = Math.Max(currentLength + previousLength + 1, maxLenghth); // +1 for the flipped bit. Please note we add the 1 because there will be a flip of 0 to 1 at some point
        num >>= 1; //Shift right
    }

    return maxLenghth;
}
#endregion

#region Next Number
static int getNextLargest(int num)
{
    var c = num;
    var c0 = 0;
    var c1 = 0;

    // Count trailing 0s, the ones that are on the rightmost side of the number starting from the least significant bit (LSB)
    while (((c & 1) == 0) && (c != 0))
    {
        c0++;
        c >>= 1;
    }

    // Count 1s after the trailing 0s
    while ((c & 1) == 1)
    {
        c1++;
        c >>= 1;
    }

    // No bigger number possible
    if (c0 + c1 == 31 || c0 + c1 == 0) //31 for 32-bit signed numbers (the sign bit is not counted) and 0 when num is 0
        return -1;

    int p = c0 + c1;

    // Flip rightmost non-trailing 0 (at position p), the one after 1s that follow trailing 0s. This we increase the number of 1s by 1 thus increasing the number itself
    num |= (1 << p);

    /*
     * We then shrink the number by rearranging all the bits to the right of bit p such that the 0s are on the left and the 1s are on the right. 
     * As we do this, we want to replace one of the 1 s with a 0. 
     * We clear all bits to the right of p (inclusive) and then insert (c1 - 1) 1s on the right.
     */
    var a = 1 << p; //all zeros except for a 1 at position p.
    var b = a - 1; //all zeros, followed by p ones.
    var mask = ~b; //all ones, followed by p zeros.
    num = num & mask; // Clear rightmost p bits

    var d = 1 << (c1 - 1); //0s with a 1 at position c1 - 1
    var e = d - 1; //0s with ls at positions 0 through c1 - 1
    num = num | e; // Insert(c1 - 1) 1s on the right

    return num;
}

static int getNextSmallest(int num)
{
    /*
     * 1. Compute c0 and c1. Note that c1 is the number of trailing ones, and c0 is the size of the block of zeros immediately to the left of the trailing ones. 
     * 2. Flip the rightmost non-trailing one to a zero. This will be at position p 
     * 3. Clear all bits to the right of bit p. 
     * 4. Insert cl + 1 ones immediately to the right of position p.
     */

    var c = num;
    var c0 = 0;
    var c1 = 0;

    // Count trailing 1s
    while ((c & 1) == 1) { 
        c1++; 
        c >>= 1; 
    }

    // No smaller number possible
    if (c == 0) return -1;

    // Count trailing 0s
    while (((c & 1) == 0) && (c != 0))
    {
        c0++;
        c >>= 1;
    }

    int p = c0 + c1;

    var a = ~0; //Sequence of ls 
    var b = a << (p + 1); //All 1s, followed by p+1 0s
    num &= b; // Clears bits 0 through p

    var d = 1 << (c1 + 1); // 0s with a 1 at position( c1 + 1)
    var e = d - 1; // 0s followed by c1 + 1 ones
    var f = e << (c0 - 1); // c1+1 ones followed by c0-1 zeros. 

    num |= f; // Position the mask correctly

    return num;
}
#endregion

#region Debugger: Explain what the following code does: ((n & (n-1 )) == 0)

// This expression checks if n is a power of two (or zero).
// It works because powers of two in binary have exactly one bit set to 1 (1, 10, 100, 1000, 10000), and n-1 will have all bits to the right of that bit set to 1.
// Therefore, performing a bitwise AND between n and n-1 will result in 0 if n is a power of two.

#endregion

#region Count Bits to Flip
static int countBitsToFlip(int A, int B)
{
    /*
     Use the bitwise XOR operator ^:

        A ^ B will give a number where each bit is:

            - 1 if A and B are different in that position

            - 0 if A and B are the same

        Count how many 1s are in that result — that’s the number of bits you’d need to flip.
     */

    var num = A ^ B; 
    int count = 0;

    while(num != 0)
    {
        count += (num & 1);
        num >>= 1;
    }

    return count;
}
#endregion

#region Odd and Even bits swap
static int SwapOddEvenBit(int x)
{
    // 0xAAAAAAAA = 10101010 10101010 10101010 10101010 in binary (32-bit)
    // This mask selects all bits in **odd positions** (1, 3, 5, ..., 31)
    var oddBits = x & 0xAAAAAAAA;

    // 0x55555555 = 01010101 01010101 01010101 01010101 in binary (32-bit)
    // This mask selects all bits in **even positions** (0, 2, 4, ..., 30)
    var evenBits = x & 0x55555555;

    // Shift odd bits right to even positions
    oddBits >>= 1;

    // Shift even bits left to odd positions
    evenBits <<= 1;

    return (int)(oddBits | evenBits);
}
#endregion

#region Draw Line
static void drawline(byte[] screen, int width, int x1, int x2, int y)
{
    int bytesPerRow = width / 8;

    var startOffset = x1 % 8;
    var startByte = (y * bytesPerRow) + (x1 / 8); //we add (x1/8) because y-axis is zero based and we need to move x1/8 bytes to the right

    var endOffset = x2 % 8;
    var endByte = (y * bytesPerRow) + (x2 / 8);

    if (startByte == endByte)
    {
        // x1 and x2 are in the same byte
        var mask = (byte)((0xFF >> startOffset) & (0xFF << (7 - endOffset))); // 0xFF = 11111111 in binary. We use 7 instead of 8 because the pixels(bits) in the byte are 0-indexed
        screen[startByte] |= mask;
    }
    else
    {
        var startMask = (byte)(0xFF >> startOffset);
        screen[startByte] |= startMask; // Set bits in the first byte

        for(int i = startByte + 1; i < endByte; i++)
        {
            screen[i] = 0xFF; // Set all bits in the middle bytes
        }

        var endMask = (byte)(0xFF << (7 - endOffset));
        screen[endByte] |= endMask; // Set bits in the last byte
    }
}
#endregion

#region XOR from 1 to n (direct method)

static int xorFrom1ToN(int n) {
    switch (n % 4) {
        case 0: return n; // If n is divisible by 4, the result is n
        case 1: return 1; // If n % 4 == 1, the result is 1
        case 2: return n + 1; // If n % 4 == 2, the result is n + 1
        case 3: return 0; // If n % 4 == 3, the result is 0
        default: throw new ArgumentException("n must be a non-negative integer.");
    }
}

#endregion

#region Count of numbers (x) smaller than or equal to n such that n+x = n^x

static int countNumbersX(int n)
{
    // Count of numbers x such that n + x = n ^ x
    // is equal to the number of bits set to 0 in n.
    int countOfZeros = 0;

    while (n > 0) {
        if ((n & 1) == 0) countOfZeros++; // Check if the least significant bit is 0
        n >>= 1; // Shift right to check the next bit
    }

    return 1 << countOfZeros; // equivalent to 2^countOfZeros
}

#endregion

#endregion

#region 6 Math and Logic Puzzles

#region Checkinng for Primality
static bool isNumberPrime(int n) {
    if(n < 2) return false; // 0 and 1 are not prime numbers

    int sqrt = (int) Math.Sqrt(n);

    for(int i = 2; i <= sqrt; i++) {
        if (n % i == 0) return false; // If n is divisible by i, it is not prime
    }

    return true; // If no divisors found, n is prime
}
#endregion

#region Sieve of Eratosthenes
static List<int> sievOfEratosthenes(int n) {
    if (n < 2) return new List<int>(); // No primes less than 2

    bool[] isPrime = new bool[n + 1];

    for(int i = 2; i <= n; i++)
    {
        isPrime[i] = true; // Assume all numbers are prime initially
    }

    for (int p = 2; p * p <= n; p++) {
        if (isPrime[p]) {
            //Mark all multiples of p as non-prime
            for (int multiple = p * p; multiple <= n; multiple += p)
            {
                isPrime[multiple] = false;
            }
        }
    }

    List<int> primes = new List<int>();
    for(int i = 2; i <= n; i++)
    {
        if (isPrime[i])
        {
            primes.Add(i);
        }
    }

    return primes; // Return the list of prime numbers
}
#endregion

#region Girls and Boys in a population
static double girlsToBoys(int n)
{
    if (n < 1) return 0.0; // If there are no people, return 0
    
    int boys = 0;
    int girls = 0;
    Random random = new Random();

    for(int i = 0; i < n; i++)
    {
        while (true)
        {
            bool isGirl = random.NextDouble() < 0.5; // Simulating a random occurrence leading to a boy or girl

            if (isGirl)
            {
                girls++;
                break; 
            }
            else
            {
                boys++;
            }
        }
    }

    return (double) boys/ girls;
}
#endregion

#region Egg Drop at N Floor problem
static bool drop(int floor, int drops, int breakingFloor)
{
    drops++;

    return floor >= breakingFloor; // Simulate the drop, returns true if it breaks
}

static int findBreakingFloor(int floors)
{
    var breakingFloor = 73;
    var drops = 0;

    var interval = 14;
    var previousFloor = 0;
    var egg1Floor = interval;

    /* Drop egg1 at decreasing intervals. */
    while (!drop(egg1Floor, drops, breakingFloor) && egg1Floor <= floors)
    {
        interval--;
        previousFloor = egg1Floor;
        egg1Floor += interval; // Increase the floor to drop the first egg
    }

    int egg2Floor = previousFloor + 1;
    while(egg2Floor < egg1Floor && egg2Floor <= floors && !drop(egg2Floor, drops, breakingFloor))
    {
        egg2Floor++; // Drop the second egg at each floor between previousFloor and egg1Floor
    }

    return egg2Floor > floors ? -1 : egg2Floor; // Return the floor where the egg breaks, or -1 if it never breaks
}
#endregion

#region Poison bottle detection
static void findPoisonBottle()
{
    const int totalBottles = 1000;
    const int totalStrips = 10;

    //Set Randomly one of the bottles to be poisoned
    Random random = new Random();
    var poisonedBottle = random.Next(1, totalBottles + 1);

    bool[] testStrips = testBottles(poisonedBottle, new bool[totalStrips], totalBottles, totalStrips);

    int detectedBottle = 0;

    for (int i = 0; i < totalStrips; i++) {
        if (testStrips[i]) {
            detectedBottle |= (1 << i); // Set the bit corresponding to the strip that tested positive
        }
    }
}

static bool[] testBottles(int poisonedBottle, bool[] testStrips, int totalBottles, int totalStrips) {
    // Simulate testing bottles with test strips
    for (int bottleNum = 1; bottleNum <= totalBottles; bottleNum++) {
        int num = bottleNum;

        // Check if bit is set in bottle number
        for (int bit = 0; bit < totalStrips; bit++) { 
            if((num & (1 << bit)) != 0)
            {
                if(bottleNum == poisonedBottle)
                    testStrips[bit] = true; // If this bottle is poisoned, set the corresponding strip to true
            }
        }
    }

    return testStrips; // Return the test strips indicating which bottles are poisoned
}
#endregion

#endregion

#region 7 Object-Oriented Design

#region Singleton Design Pattern
var firstInstance = SingletonClass.GetInstance();
var secondInstance = SingletonClass.GetInstance();

bool isSingleton = firstInstance == secondInstance ; // Should be true
#endregion

#region Factory Design Pattern

FactoryClass firstFactory = FactoryMethod.CreateFactoryClassInstance(FactoryType.FirstInstance);
firstFactory.ShortIntro();

FactoryClass secondFactory = FactoryMethod.CreateFactoryClassInstance(FactoryType.SecondInstance);
secondFactory.ShortIntro();

#endregion

#region Deck of Cards
var deck = new BlackjackDeck();
deck.Shuffle();

var playerHand = new BlackjackHand();
playerHand.AddCard(deck.DealCard());
playerHand.AddCard(deck.DealCard());

Console.WriteLine(playerHand.ToString());
Console.WriteLine(playerHand.IsBusted ? "Busted" : "Still in the game");
#endregion

#region Call center employees
CallHandler handler = new CallHandler();

handler.AddEmployee(new Respondent("Alice"));
handler.AddEmployee(new Manager("Bob"));
handler.AddEmployee(new Director("Charlie"));

//Simulate incoming calls
handler.DispatchCall(new Call(EmployeeRank.Respondent));
handler.DispatchCall(new Call(EmployeeRank.Manager));
handler.DispatchCall(new Call(EmployeeRank.Director));
handler.DispatchCall(new Call(EmployeeRank.Respondent));
#endregion

#region Jukebox design
var jukebox = new JukeBox();

// Add some songs
jukebox.AddSongToLibrary(new Song("My song", "Peteclaver", "Songs", 50));
jukebox.AddSongToLibrary(new Song("My song 2", "Peteclaver", "Songs", 70));

// Create user
var user = new User("John Doe", 3);

// Show available songs
Console.WriteLine("\nAvailable songs:");
jukebox.showLibrary();

// Select songs
jukebox.AddSongToPlaylist(user, "My Song");
jukebox.AddSongToPlaylist(user, "No song");

// Play songs
jukebox.PlayNextSong();
#endregion

#region Parking Lot design
var parkingLot = new ParkingLot(2, 3, 10);

var motorCycle = new MotorCycle();
var car = new Car();
var bus = new Bus();

Console.WriteLine("Motorcycle parked: " + parkingLot.ParkVehicle(motorCycle));
Console.WriteLine("Car parked: " + parkingLot.ParkVehicle(car));
Console.WriteLine("Bus parked: " + parkingLot.ParkVehicle(bus));
#endregion

#region Book reader
var system = new BookReaderSystem();

system.RegisterUser(1, "Peter");
system.RegisterUser(2, "John");

system.AddBook(new Book(1, "First Book", "Peterclaver", new string('A', 500)));
system.AddBook(new Book(2, "Second Book", "Kimuli", new string('B', 400)));

system.LoginUser(1);

system.SearchBooks("First");
system.ReadBook(1);

system.LogoutUser();
#endregion

#region Chat server
//Check implemented class
#endregion

#region Othello game

var game = new Game();

while (!game.IsGameOver)
{
    game.ShowBoard();

    Console.WriteLine("Enter row and col (e.g. 2 3): ");

    var input = Console.ReadLine()?.Split();
    if (input == null || input.Length < 2) continue;

    int row = int.Parse(input[0]);
    int col = int.Parse(input[1]);

    game.PlayTurn(row, col);
}

game.ShowBoard();
game.ShowWinner();

#endregion

#region Circular Array

var circularArray = new CircularArray<string>(4);
circularArray[0] = "A";
circularArray[1] = "B";
circularArray[2] = "C";

circularArray.Rotate(1);

Path.GetFileName("/home");
#endregion

#region In memory file system
//Check the class
#endregion

#region Hash table 
var table = new HashTable<int, string>(10);

table.Add(1, "Peter");
table.Add(2, "Kimuli");
table.Add(3, "Claver");

table.Remove(1);
table.PrintAll();
#endregion

#region Minesweep game
var mineSweeper = new MineSweeperGame(8, 10);
mineSweeper.Run();
#endregion

#endregion

#region 8 Recursion and Dynamic Programming

#region Top-Down Approach: Memoization

var memo = new Dictionary<int, int>();

int Fib(int x)
{
    if (x <= 1) return x;

    if (memo.ContainsKey(x)) return memo[x];

    memo[x] = Fib(x - 1) + Fib(x - 2);

    return memo[x];
}

#endregion

#region Bottom-Up Approach
int FibBA(int x)
{
    if(x <= 1) return x;

    int[] dp = new int[x + 1];
    dp[0] = 0;
    dp[1] = 1;

    for(int i = 2; i <= x; i++)
        dp[i] = dp[i - 1] + dp[i - 2];

    return dp[x];
}
#endregion

#region Triple step

long TripStep(int num)
{
    if (num < 0) return 0;
    if (num == 0) return 1;

    long[] dp = new long[num + 1];
    dp[0] = 1; // 1 way to stay at the bottom
    dp[1] = 1; // only one way: take 1 step.

    if (num >= 2) dp[2] = 2; // two ways: (1,1) or (2)

    for (int i = 3; i <= num; i++)
    {
        dp[i] = dp[i - 1] + dp[i -2] + dp[i - 3];
    }

    return dp[num];
}

long TripleStepOpt(int num)
{
    if (num < 0) return 0;
    if (num == 0) return 1;
    if (num == 1) return 1;
    if (num == 2) return 2;

    long a = 1, b = 1, c = 2;

    for(int i = 3; i <= num; i++)
    {
        long total = a  + b + c;
        a = b; b = c; c = total;
    }

    return c;
}
#endregion

#region Robot in a Grid

var grid = new bool[,]
{
    {true, true, true},
    {true, false, false},
    {true, true, true}
};

var robot = new RobotGrid(grid);
var path = robot.GetPath();

if(path != null)
    path.Reverse(); // Because we added points in reverse order

foreach(var p in path)
    Console.WriteLine(p);

#endregion

#region Magic Index

int FindMagicIndex(int[] arr)
{
    return BinarySearch(arr, 0, arr.Length-1);
}

int BinarySearch(int[] array, int start, int end)
{
    if (start > end)
        return -1;

    int mid = (start + end) / 2;

    if (array[mid] == mid) 
        return mid;
    else if (array[mid] > mid) 
        return BinarySearch(array, start, mid - 1);
    else 
        return BinarySearch(array, mid + 1, end);
}

int FindMagicIndexWithDups(int[] arr)
{
    return BinarySearchWithDups(arr, 0, arr.Length-1);
}

int BinarySearchWithDups(int[] arr, int start, int end)
{
    if (start > end) return -1;

    int mid = (start + end) / 2;
    int midVal = arr[mid];

    if (mid == midVal) return mid;

    //Search left
    var leftEnd = Math.Min(mid - 1, midVal);
    var left = BinarySearchWithDups(arr, start, leftEnd);
    if(left != -1) return left;

    //search right
    var rightStart = Math.Max(mid + 1, midVal);
    return BinarySearchWithDups(arr, rightStart, end);
}

/*
 * Time Complexity: O(log n) best case, but O(n) in the worst case when duplicates force both sides to be explored.

 * Space Complexity: O(log n) recursion stack (in the best case; up to O(n) in the worst case).
 */

#endregion

#region Power Set: Subsets

List<List<int>> GetSubsets(List<int> set, int index) //Index starts at 0 and goes upto set.Count. Index represents the current index in the list being processed
{
    var allSubsets = new List<List<int>>();

    if(index == set.Count)
        allSubsets.Add(new List<int>());
    else
    {
        allSubsets = GetSubsets(set, index + 1); //We keep incrementing index until we reach the base case
        var item = set[index];

        var moreSubsets = new List<List<int>>();

        foreach(var subset in allSubsets)
        {
            var newSubset = new List<int>(subset) { item }; // we create a new subset so we don't modify the existing one

            moreSubsets.Add(newSubset);
        }

        allSubsets.AddRange(moreSubsets);  // combine the two lists
    }

    return allSubsets;
}

/*
 * Time: O(2^n.n) → each element has two choices (in/out), and copying subsets costs up to O(n) each
 * Space: O(2^n.n) for storing all subsets, plus recursion stack of O(n).
 */

#endregion

#region Recursive Multiply

int Multiply(int a, int b)
{
    // Optimize by always multiplying the smaller number
    int smaller = Math.Min(a, b);
    int bigger = Math.Max(a, b);

    return RecursiveMultiply(smaller, bigger);
}

int RecursiveMultiply(int smaller, int bigger)
{
    if(smaller == 0) return 0;
    if(smaller == 1) return bigger;

    int half = smaller >> 1; // divide by 2. We divide the smaller number to minimize the number of recursive calls
    int multiplyHalf = RecursiveMultiply(half, bigger);

    if(smaller % 2 == 0)
        return multiplyHalf + multiplyHalf;
    else
        return multiplyHalf + multiplyHalf + bigger; //We add the bigger number once more if smaller is odd because we divided it by 2 and lost the remainder of 1
}

#endregion

#region Towers of Hanoi

var source = new HanoiTowers("A");
var buffer = new HanoiTowers("B");
var destination = new HanoiTowers("C");

for (int i = 0; i < 3; i++)
    source.Add(i); // Add disks 3, 2, 1

source.MoveDisks(3, destination, buffer);

#endregion

#region Permutations without Dups **Revisit this**

List<string> GetPermutations(string str)
{
    var result = new List<string>();

    Permute(str, "", result);

    return result;
}

void Permute(string remaining, string path, List<string> results)
{
    if (remaining == null)
        return;

    if (remaining.Length == 0)
        results.Add(path);
    else
    {
        for(int i = 0; i < remaining.Length; i++)
        {
            var current = remaining[i];
            var before = remaining.Substring(0, i);
            var after = remaining.Substring(i + 1);
            var rest = before + after;

            // Recurse with one character added to path, and that char removed from remaining
            Permute(rest, path + current, results);
        }
    }
}

/*
 * Time complexity: O(n.n!) Because for each of the n! permutations, you spend up to O(n) doing substring + concatenation..
 * Space complexity: O(n.n!) (dominated by the result list; recursion adds only O(n) extra).
 */

#endregion

#region Permutations with Duplicates **Revisit this**

IList<string> GetPermutationsWithDups(string str)
{
    var results = new List<string>();
    var freqMap = BuildFrequencyMap(str); // Frequency map to count occurrences of each character since we have duplicates
    GeneratePermutations(freqMap, "", str.Length, results); //str.Length is the total number of characters to be used in each permutation
    return results;
}

Dictionary<char, int> BuildFrequencyMap(string str)
{
    var map = new Dictionary<char, int>();
    foreach (char c in str)
    {
        if (!map.ContainsKey(c))
        {
            map[c] = 0;
        }

        map[c]++;
    }

    return map;
}

void GeneratePermutations(Dictionary<char, int> freqMap, string prefix, int remaining, List<string> results)
{
    if(remaining == 0)
    {
        results.Add(prefix);
        return;
    }

    foreach (var entry in freqMap) { 
        char ch = entry.Key;
        int count = entry.Value;

        if(count > 0)
        {
            freqMap[ch]--;
            GeneratePermutations(freqMap, prefix + ch, remaining - 1, results);
            freqMap[ch]++; // backtracking because we modified the map and we need to restore it for the next iteration
        }
    }
}

#endregion

#region Parens

List<string> GetParentheses(int n)
{
    var result = new List<string>();
    GenerateParens(n, n, "", result);
    return result;
}

void GenerateParens(int left, int right, string current, List<string> result)
{
    // Base case: no more parens to add
    if (left == 0 && right == 0)
    {
        result.Add(current);
        return;
    }

    // Add a '(' if we still have one
    if (left > 0)
        GenerateParens(left - 1, right, current + '(', result);

    // Add a ')' if we can match with a previous '('
    if (right  > left)
        GenerateParens(left, right - 1, current + ')', result);
}

/*
 * Time complexity: O(2ⁿ) 
 * Space: O(n)
 */

#endregion

#region Paint/Flood fill

void PaintFill(int[][] screen, int r, int c, int nColor)
{
    int oColor = screen[r][c];

    if (oColor == nColor) return; // Avoid infinite loop

    FillHelper(screen, r, c, oColor, nColor);
}

//int[][] screen represents a 2D array and is jagged array (array of arrays) in C#. Each row can have different lengths.
//int[,] screen is a rectangular 2D array where all rows have the same length. Used in image grids or matrices.

void FillHelper(int[][] screen, int r, int c, int oColor, int nColor) 
{
    // Check boundaries
    if (r < 0 || r >= screen.Length || c < 0 || c >= screen[0].Length)
    {
        return;
    }

    // Stop if color is not the one we're replacing
    if (screen[r][c] != oColor)
        return;

    // Fill current cell
    screen[r][c] = nColor;

    // Recursively fill neighbors
    FillHelper(screen, r + 1, c, oColor, nColor);
    FillHelper(screen, r - 1, c, oColor, nColor);
    FillHelper(screen, r, c + 1, oColor, nColor);
    FillHelper(screen, r, c - 1, oColor, nColor);
}

#endregion

#region Coins Change

int[] coins = new int[] { 25, 10, 5, 1 };

int waysToMakeChange(int amount)
{
    var memo = new Dictionary<string, int>();
    return countWays(amount, 0, memo);
}

int countWays(int amount, int index, Dictionary<string, int> memo)
{
    if(index == coins.Length - 1)
        return 1; // Only one way to make change with the last coin

    string key = amount + "-" + index;

    if (memo.ContainsKey(key))
    {
        return memo[key];
    }

    int coin = coins[index];
    int ways = 0;

    // Try all values from 0 to amount using this coin
    for (int i = 0; i * coin <= amount; i++)
    {
        int remaining = amount - (i * coin);
        ways += countWays(remaining, index + 1, memo);
    }

    memo[key] = ways; // Store the result in the memoization dictionary
    return ways;
}

//Time complexity: O(n * m) where n is the amount and m is the number of coins.
//Space complexity: O(n * m) for the memoization dictionary.

#endregion

#region Eight Queens
int SIZE = 8;

void PlaceQueens(int row, int[] positions, HashSet<int> columns, HashSet<int> diag1, HashSet<int> diag2, List<int[]> results)
{
    if(row == SIZE)
    {
        results.Add((int[])positions.Clone()); // Add a copy of the current positions
        return;
    }

    for (int col = 0; col < SIZE; col++)
    {
        if(columns.Contains(col) || diag1.Contains(row - col) || diag2.Contains(row + col)) // row - col is the same for \ diagonal, row + col is the same for / diagonal
            continue; // Skip if the column or diagonals are already occupied

        positions[row] = col; // Place the queen
        columns.Add(col);
        diag1.Add(row - col);
        diag2.Add(row + col);

        PlaceQueens(row + 1, positions, columns, diag1, diag2, results); // Recur to place the next queen

        //Backtrack - remove the queen and free up the column and diagonals because we are going to try the next column in the current row
        columns.Remove(col);
        diag1.Remove(row - col);
        diag2.Remove(row + col);
    }
}

#endregion

#region Stack of Boxes

var solution = new BoxStackSolution();

#endregion

#region Boolean Evaluation **Its very crucial that you revisit this**

// Cache previously computed results to avoid redundant work
var solutionCache = new Dictionary<string, int>();

int CountWays(string exp, bool result)
{
    if(exp.Length == 0) return 0; // Base case: No expression to evaluate

    if (exp.Length == 1) { 
        var value = exp == "1";
        return value == result ? 1 : 0; // Base case: If the single character matches the desired result, return 1, else 0
    }

    //Use a memo key based on expression + desired result
    var key = exp + "_" + result;

    if (solutionCache.ContainsKey(key)) { 
        return solutionCache[key]; // Return cached result if available
    }

    int ways = 0;

    // Loop through operators only (index 1, 3, 5, ...)
    for (int i = 1; i < exp.Length; i += 2)
    {
        var op = exp[i]; // Get the operator

        var left = exp.Substring(0, i); // Left sub-expression
        var right = exp.Substring(i + 1); // Right sub-expression

        // Count ways for left and right sub-expressions
        var leftTrue = CountWays(left, true);
        var leftFalse = CountWays(left, false);
        var rightTrue = CountWays(right, true);
        var rightFalse = CountWays(right, false);

        // Total combinations of left and right evaluations
        int totalWays = (leftTrue + leftFalse) * (rightTrue + rightFalse); // We multiply because each left result can pair with each right result

        /*
         * Think of the expression above like outfits:

           Left side = number of shirts

           Right side = number of pants

           A complete outfit = pick 1 shirt and 1 pair of pants.

           👉 If you have 3 shirts and 5 pants, you don’t have 3 + 5 = 8 outfits, you have 3 × 5 = 15 outfits, because every shirt can pair with every pant.
         */

        // Number of combinations that result in TRUE based on the operator
        int totalTrueWays = 0;

        if (op == '&') {
            totalTrueWays = leftTrue * rightTrue; //Both must be true
        }
        else if (op == '^')
        {
            totalTrueWays = (leftTrue * rightFalse) + (leftFalse * rightTrue);
        }
        else if (op == '|')
        {
            totalTrueWays = (leftTrue * rightTrue) + (leftTrue * rightFalse) + (leftFalse * rightTrue);
        }

        // If we want result to be true, count totalTrue
        // Else, subtract from total to get number of false combinations
        int subWays = result ? totalTrueWays : totalWays - totalTrueWays;

        ways += subWays; // Add to the total ways
    }

    // Memoize result for future calls
    solutionCache[key] = ways;

    return ways; // Return the total number of ways to evaluate the expression to the desired result
}

#endregion

#endregion

#region 9 System Design

#region Stock Data system design

/* Recommended Approach: RESTful API with Caching + CDN
 * Data Model:
    Store data in a key-value format:
        Key: {stock_symbol}:{date}
        Value: {open, close, high, low}
    
    - Use something like Redis or SQL with indexing for fast retrieval.

 * API Design: GET /api/stock/{symbol}/eod?date=2025-07-29
 * 
 * | Component      | Tech Choice                                     |
| -------------- | ----------------------------------------------- |
| API Gateway    | NGINX / AWS API Gateway                         |
| Backend API    | ASP.NET Core / Node.js / Python Flask           |
| Caching Layer  | Redis (in-memory store)                         |
| Storage        | PostgreSQL / DynamoDB                           |
| CDN (optional) | Cloudflare / AWS CloudFront                     |
| Monitoring     | Prometheus + Grafana / AWS CloudWatch           |
| Deployment     | Docker + Kubernetes or Serverless (e.g. Lambda) |

Scalability Plan:
- Preload Redis with EOD data before market close

- Use API gateway rate limiting and caching

- Serve high-traffic symbols (e.g. AAPL, MSFT) via CDN-backed JSON endpoints

- Auto-scale backend pods using Kubernetes or serverless

Monitoring & Maintenance:
    Monitor:

    - Request volume and latency

    - Cache hit/miss ratio

    - API error rates

    Alerts on:

    - Latency spikes

    - Backend failure

 */

#endregion

#region cache design

var store = new TimedKeyValueStore<string, int>(
    ttl: TimeSpan.FromMinutes(10),
    isSlidingExpiration: false,
    maxCapacity: 1000
    );

//Add
store.AddOrUpdate("TestString", 1);

Array.Copy(new int[5], 2, new int[4], 2, 2);

//Get
if(store.TryGetValue("TestString", out var value))
{
    Console.WriteLine(value);
}

// Purge expired (e.g., on a timer)
int removedCount = store.PurgeExpired();
Console.WriteLine($"Purgeed {removedCount} expired items");

#endregion

#endregion

#region 10 Sorting and Searching

#region Merge Sort
void SortArray(int[] arr)
{
    if (arr.Length <= 1 || arr == null) return;

    var temp = new int[arr.Length];
    Sort(arr, temp, 0, arr.Length - 1);
}

void Sort(int[] arr, int[] temp, int left, int right)
{
    if (left < right)
    {
        int mid = left + (right - left) / 2; 

        //Sort left half
        Sort(arr, temp, left, mid);

        //Sort right half
        Sort(arr, temp, mid + 1, right);

        // Small optimization: if the biggest in left <= smallest in right,
        // they are already in order; skip merge.
        if (arr[mid] <= arr[mid + 1]) return; // Already sorted

        //Merge both halves
        Merge(arr, temp, left, mid, right);
    }
}

void Merge(int[] arr, int[] temp, int left, int mid, int right)
{
    //copy arr into temp
    for (int i = left; i <= right; i++)
    {
        temp[i] = arr[i];
    }

    int leftTempPointer = left; // pointer into left half in temp
    int rightTempPointer = mid + 1; // pointer into right half in temp
    int currentIndex = left;    // position to write into a

    while (leftTempPointer <= mid && rightTempPointer <= right)
    {
        if (temp[leftTempPointer] <= temp[rightTempPointer])
        {
            arr[currentIndex++] = temp[leftTempPointer++];
        }
        else
        {
            arr[currentIndex++] = temp[rightTempPointer++];
        }
    }

    // Copy any remaining elements from the left half
    while (leftTempPointer <= mid)
    {
        arr[currentIndex++] = temp[leftTempPointer++];
    }

    // Leftovers from the right half are already in place in 'arr'
    // (because we copied from 'arr' to 'temp' and write 'a' left-to-right).
}

//Complexity: time ≈ O(n log n), extra space ≈ O(n) for the temp buffer.
#endregion

#region Quick Sort
void QuickSort(int[] arr, int left, int right)
{
    int index = PartitionArr(arr, left, right);

    //Sort left half
    if(left < index - 1) //this condition is to avoid infinite recursion when index == left
    {
        QuickSort(arr, left, index - 1);
    }

    //Sort right half
    if(index < right)
    {
        QuickSort(arr, index, right);
    }
}

int PartitionArr(int[] arr, int left, int right)
{
    int pivot = arr[(left + right) / 2]; // Choose the middle element as pivot

    while (left <= right) {
        // Find item on left that should be on right
        while (arr[left] < pivot) left++;

        // Find item on right that should be on left
        while (arr[right] > pivot) right--;

        //Swap values, then move both pointers
        if (left <= right)
        { //we use <= to handle the case when left == right, so we swap the same element and move both pointers to avoid infinite loop when the partitioning is done again
            Swap(arr, left, right);
            left++;
            right--;
        }
    }

    return left; // New partition boundary
}

void Swap(int[] arr, int left, int right)
{
    var temp = arr[left];
    arr[left] = arr[right];
    arr[right] = temp;
}

//Runtime: O(n log( n)) average, O(n^2) worst case. Memory: 0(log(n))
#endregion

#region Iterrative Binary Search

int IterativeBinarySearch(int[] arr, int target)
{
    int left = 0;
    int right = arr.Length - 1;
    int mid;

    while (left <= right)
    {
        mid = (right + left) / 2;

        if (arr[mid] < target)
            left = mid + 1;
        else if (arr[mid] > target)
            right = mid - 1;
        else
            return mid;
    }

    return -1; // Not found
}
#endregion

#region Recursive Binary Search
int RecursiveBinarySearch(int[] arr, int target, int left, int right)
{
    if (left > right) return -1; //Not found

    int mid = (left + right) / 2;

    if (arr[mid] < target)
        return RecursiveBinarySearch(arr, target, mid + 1, right);
    else if (arr[mid] > target)
        return RecursiveBinarySearch(arr, target, left, mid - 1);
    else
        return mid;
}

#endregion

#region Sorted Merge

void SortedMerge(int[] A, int[] B, int lastA, int lastB)
{
    // lastA = number of "real" elements in A
    // lastB = number of elements in B

    int pointerA = lastA - 1;   // last real element in A
    int pointerB = lastB - 1;   // last element in B
    int mergedArrPointer = lastA + lastB - 1;   // end of merged array (buffer space included)

    // Merge A and B, starting from the end
    while (pointerB >= 0)
    {
        if(pointerA >= 0 && A[pointerA] > B[pointerB])
        {
            // A's element is bigger → place it at the end
            A[mergedArrPointer] = A[pointerA];
            pointerA--;
        }
        else
        {
            // B's element is bigger → place it at the end
            A[mergedArrPointer] = B[pointerB];
            pointerB--;
        }

        mergedArrPointer--;
    }
}

#endregion

#region Group Anagrams

string[] GroupAnagrams(string[] arr)
{
    // Dictionary: key = sorted word, value = list of anagrams
    var map = new Dictionary<string, List<string>>();

    foreach(var word in arr)
    {
        // Step 1: Sort the characters in the word
        var chars = word.ToCharArray();
        Array.Sort(chars);
        var key = new string(chars); // e.g. "eat" → "aet"

        // Step 2: Add to the dictionary
        if (!map.ContainsKey(key))
            map[key] = new List<string>();

        map[key].Add(word);
    }

    // Step 3: Flatten all values into a single array
    var result = new List<string>();

    foreach (var group in map.Values)
        result.AddRange(group);

    return result.ToArray();
}

/*
 * Time: O(n · k log k)
 * Space: O(n · k)
 */

#endregion

#region Search in Rotated Array ** Revisit this **

int SearchRotatedArr(int[] arr, int target)
{
    int left = 0, right = arr.Length - 1;

    while(left <= right)
    {
        int mid = left + (right - left) / 2;

        // 🎯 Found the target
        if (arr[mid] == target)
            return mid;
        
        // 🟡 If we cannot decide because of duplicates
        if (arr[left] == arr[mid] && arr[right] == arr[mid])
        {
            left++;
            right--;
        }
        // 🟢 Check if left half is sorted
        else if (arr[left] <= arr[mid])
        {
            // Is target within the sorted left half?
            if (arr[left] <= target && target < arr[mid])
                right = mid - 1; // search left
            else
                left = mid + 1; // search right
        }
        // 🔵 Else, the right half must be sorted
        else
        {
            // Is target within the sorted right half?
            if (arr[right] >= target && target > arr[mid])
                left = mid + 1; // search right
            else
                right = mid - 1; // search left
        }
    }

    return -1; // not found
}

/*
Average case: O(log n), Worst case (with many duplicates): O(n)
Space: O(1) → just a few pointers
 */

#endregion

#region Sorted Search, No Size
int ListySearch(Listy list, int target)
{
    int index = 1;

    // 🔹 Step 1: Find bounds
    while (list.ElementAt(index) != -1 && list.ElementAt(index) < target)
        index *= 2;

    // 🔹 Step 2: Binary search between index/2 and index
    return BinarySearchListy(list, target, index / 2, index);
}

int BinarySearchListy(Listy list, int target, int low, int high) { 
    while(low <= high)
    {
        int mid = (low + high) / 2;
        int value = list.ElementAt(mid);

        if (value == -1 || value > target)
            high = mid - 1;
        else if (value < target)
            low = mid + 1;
        else
            return mid; // Found
    }

    return -1; //not found
}

#endregion

#region Sparse Search

int SparseSearch(string[] arr, string target)
{
    if (arr == null || target == "" || target == null)
        return -1;

    return SparseSearchHelper(arr, target, 0, arr.Length - 1);
}

int SparseSearchHelper(string[] arr, string target, int low, int high)
{
    while(low <= high)
    {
        int mid = (low + high) / 2;

        if (arr[mid] == "")
        {
            int left = mid - 1;
            int right = mid + 1;

            while (true)
            {
                if (left < low && right > high)
                    return -1;
                else if (right <= high && arr[right] != "")
                {
                    mid = right;
                    break;
                }
                else if(left >= low && arr[left] != ""){
                    mid = left;
                    break;
                }
                right++;
                left--;
            }
        }

        if (arr[mid] == target)
            return mid;
        else if (arr[mid].CompareTo(target) < 0)
            low = mid + 1;
        else
            high = mid - 1;
    }

    return -1;
}

/*

Complexity

Best case: O(log n)(just like binary search, if we rarely hit empty strings).

Worst case: O(n)(if the array has mostly empty strings and we have to scan far each time).

*/

#endregion

#region Missing Int

long numberOfInts = (long)int.MaxValue + 1; //We add 1 to include 0 as well
byte[] bitfield = new byte[(int)(numberOfInts / 8)]; // numberOfInts corresponds to number of bits we need and since computers store in bytes not bits, we convert the bits to bytes by dividing by 8
string fileName = "numbers.txt"; //input files... can be any file with the numbers

void FindMissingNumber()
{
    using StreamReader sr = new StreamReader(fileName);
    string? line;
    while ((line = sr.ReadLine()) != null)
    {
        if (int.TryParse(line, out int value))
        {
            /* Finds the corresponding byte index in the bitfield by using the divison operator. Use the % operator to get the bit in the index found.
             * To set to 1 the nth bit of a byte index, first left shift by the bit value and then use the OR operator (e.g., 10 would correspond to the 2nd bit of 
             * index 2 in the byte array). */
            
            var index = value / 8; // Finds the corresponding byte index in the bitfield by using the divison operator
            var bitInIndex = value % 8; // Use the % operator to get the bit in the index found.
            var mask = (byte)(1 << bitInIndex); // left shift by the bit value to have a 1


            bitfield[index] |= mask; //set to 1 the nth bit of a byte index
        }
    }

    // Now find the first number missing
    for(int i = 0; i < bitfield.Length; i++)
    {
        for(int j = 0; j < 8; j++)
        {
            /* Retrieves the individual bits of each byte. When 0 bit is found, print 
             * the corresponding value. */
            if ((bitfield[i] & (1 << j)) == 0)
            {
                Console.WriteLine(i * 8 + j);
                return;
            }
        }
    }
}

#region FOLLOW UP: We have only 10 MB memory and 1 billion distinct non-negative numbers

int FindOpenNumber(string fileName)
{
    int rangeSize = (1 << 20); // 2^20 bits (2^17 bytes)

    // Get count of number of values within each block
    int[] blocks = GetCountPerBlock(fileName, rangeSize);

    //Find a block with a missing value
    int blockIndex = FindBlockWithMissing(blocks, rangeSize);
    if(blockIndex < 0) return -1;

    //Create a bit vector for items within this range
    byte[] bitVector = GetBitVectorForRange(fileName, blockIndex, rangeSize);

    //Find a zero in the bit vector
    int offset = FindZeroInBitVector(bitVector);
    if (offset < 0) return -1;

    //Compute missing value
    return blockIndex * rangeSize + offset;
}

/* Get count of items within each range. */
int[] GetCountPerBlock(string fileName, int rangeSize)
{
    int arraySize = (int.MaxValue/rangeSize) + 1;
    int[] blocks = new int[arraySize];

    var sr = new StreamReader (fileName);
    string? line;

    while((line = sr.ReadLine()) != null)
    {
        if(int.TryParse(line, out int value))
        {
            blocks[value/rangeSize]++;
        }
    }

    sr.Close();
    return blocks;
}

/* Find a block whose count is low. */
int FindBlockWithMissing(int[] blocks, int rangeSize)
{
    for(int i = 0; i < blocks.Length; i++)
    {
        if (blocks[i] < rangeSize)
        {
            return i;
        }
    }

    return -1;
}

/* Create a bit vector for the values within a specific range. */
byte[] GetBitVectorForRange(string fileName, int blockIndex, int rangeSize)
{
    int startRange = blockIndex * rangeSize;
    int endRange = startRange + rangeSize;
    byte[] bitVector = new byte[rangeSize/8];

    using StreamReader sr = new StreamReader(fileName);
    string? line;

    while((line = sr.ReadLine()) != null)
    {
        if(int.TryParse(line, out int value))
        {
            /* If the number is inside the block that's missing numbers, we record it */
            if (startRange <= value && value < endRange)
            {
                var offSet = value - startRange;
                int mask = (1 << (offSet % 8));
                bitVector[offSet/8] |= (byte)mask;
            }
        }
    }

    sr.Close();

    return bitVector;
}


/* Find a zero within the bit vector and return the index. */
int FindZeroInBitVector(byte[] bitVector)
{
    for(int i = 0; i < bitfield.Length;i++)
    {
        if ((bitVector[i] & ~0) != 1) // If not all 1s
        {
            int bitIndex = FindZeroInByte(bitVector[i]);
            return i * 8 + bitIndex;
        }
    }

    return -1;
}


/* Find bit index that is 0 within byte. */
int FindZeroInByte(byte b)
{
    for(int i = 0; i < 8; i++)
    {
        var mask = (1 << i);
        if ((b & mask) == 0)
            return i;
    }

    return -1;
}

#endregion

#endregion

#region Find Duplicates ** Revisit this **
//We got 4kb of memory and array can contain as many as 32,000 numbers starting from 1

void FindDuplicatedNums(int[] numbers)
{
    int[][] arr = new int[numbers.Length][]; //USELESS

    // 4 KB = 4096 bytes = 1024 ints (since each int = 4 bytes)
    var bitVector = new int[1024]; // This translates to memory of 1024 × 32 = 32,768 bits since each int is 4 bytes and each byte is 8 bits

    foreach(int num in numbers)
    {
        var index = (num - 1) / 32; // find which int holds the bit
        var bit = (num - 1) % 32; // find which bit within the int holds the bit

        var mask = (1 << bit);

        if ((bitVector[index] & mask) != 0)
            // bit already set -> duplicate found
            Console.WriteLine(num);
        else
            // mark this number as seen
            bitVector[index] |= mask;
    }
}

/*
 * Final Complexity:

 * Time → O(N)

 * Space → O(N) bits (≈ 4 KB for max N = 32,000)
 */

#endregion

#region Sorted Matrix Search

(int, int) SearchMatrix(int[,] matrix, int target)
{
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);

    int row = 0;
    int col = cols - 1; // start at top-right corner

    while(row < rows && col >= 0)
    {
        if (matrix[row, col] == target)
        {
            return (row, col);
        }
        else if (matrix[row, col] > target)
            col--; // move left because we are at the higher end of cols and the target is on lower end
        else
            row++; // move down because we are on the lower end of the rows and yet target is at the higher end
    }

    return (-1, -1); // not found
}

/*
 * ⏱ Complexity

 *  Worst case: you move at most M + N steps.

 *  Time complexity: O(M + N)

 *  Space complexity: O(1)
 */

#endregion

#region Rank from Stream

RankNode root;

void Track(int number)
{
    if(root is null)
    {
        root = new RankNode(number);
    }
    else
    {
        root.Track(number);
    }
}

int GetRankOfNumber(int number)
{
    return root.GetRank(number);
}

/*
 * Time: The track method and the getRankOfNumber method will both operate in O(log N) on a balanced tree and O(N) on an unbalanced tree. 
 * Space: O(N) for storing nodes.
 */
#endregion

#region Peaks and Valleys

void SortIntoPeaksAndValleys(int[] arr)
{
    for(int i = 1; i < arr.Length; i += 2) // only check odd indices (peaks)
    {
        int biggerIndex = GetMaxIndex(arr, i - 1, i, i + 1);

        if(biggerIndex != i)
        {
            // Swap arr[i] with the largest neighbor
            int temp = arr[i];
            arr[i] = arr[biggerIndex];
            arr[biggerIndex] = temp;
        }
    }
}

int GetMaxIndex(int[] arr, int a, int b, int c)
{
    int len = arr.Length;

    int valA = (a >= 0 && a < len) ? arr[a] : int.MinValue;
    int valB = (b >= 0 && b < len) ? arr[b] : int.MinValue;
    int valC = (c >= 0 && c < len) ? arr[c] : int.MinValue;

    int maxVal = Math.Max(valA, Math.Max(valB, valC)); //Math.Max only takes two parameters

    if (maxVal == valA) return a;
    if (maxVal == valB) return b;
    return c;
}

/*
 * Complexity

 * We pass through the array once → O(n) time.

 * Only constant swaps → O(1) extra space.
 */

#endregion

#endregion

#region Moderate Questions

#region Number swapper

//For ints only but can overflow for large numbers
void Swapper(ref int a, ref int b)
{
    //assuming b is larger
    b = b - a;
    a = a + b;
    b = a - b;
}

//For all data types
void SwapAlt(ref int a, ref int b)
{
    a = a ^ b;
    b = a ^ b;
    a = a ^ b;
}

#endregion

#region Word frequencies

// Part 1
int GetFrequency(string book, string word)
{
    var count = 0;
    var words = book.Split(' ', '\n', ',', '.', '?', '-', '"', '\t', '!', ':');

    foreach(var w in words)
    {
        if(String.Equals(word, w, StringComparison.OrdinalIgnoreCase))
            count++;
    }

    return count;
}

//Part 2
Dictionary<string, int> CountFrequency(string book)
{
    var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    var words = book.Split(' ', '\n', ',', '.', '?', '-', '"', '\t', '!', ':');

    foreach(var w in words)
    {
        if(w != "")
        {
            if (map.ContainsKey(w))
                map[w] += 1;
            else
                map[w] = 1;
        }
    }

    return map;
}

int GetFrequencies(Dictionary<string, int> map, string word)
{
    if (map is null || word is null) return -1;

    word = word.ToLower();

    if(map.ContainsKey(word))
        return map[word];

    return 0;
}

#endregion

#region Line Intersection

Point getIntersection(Point FirstStart, Point FirstEnd, Point SecondStart, Point SecondEnd)
{
    bool isBetween(double a, double b, double c) => c >= Math.Min(a, b) && c <= Math.Max(a, b);

    //First line points
    var fx1 = FirstStart.X;
    var fy1 = FirstStart.Y;
    var fx2 = FirstEnd.X;
    var fy2 = FirstEnd.Y;

    //Second line points
    var sx1 = SecondStart.X;
    var sy1 = SecondStart.Y;
    var sx2 = SecondEnd.X;
    var sy2 = SecondEnd.Y;

    //Check if lines are vertical
    var isFVertical = fx1 == fx2;
    var isSVertical = sx1 == sx2;

    // Case 1: Both lines vertical (parallel, no unique intersection)
    if (isFVertical && isSVertical)
    {
        if(fx1 != sx1)
            return null;

        //Gets the maximum of the two startings Ys from both lines
        var overlapY = Math.Max(fy1, sy1);

        //Checks if the overlap Y coord is less than the minimum of the end point Ys
        if(overlapY <= Math.Min(fy2, sy2))
            return new Point(fx1, overlapY);

        return null;
    }
        

    double fm = 0, sm = 0, fc = 0, sc = 0;

    // Compute gradients(m) and intercepts(c) if not vertical, y = mx + c, m = (y2 - y1) / (x2 - x1)
    if (!isFVertical)
    {
        fm = (fy2 - fy1) / (fx2 - fx1);
        fc = fy1 - (fm * fx1);
    }

    if (!isSVertical)
    {
        sm = (sy2 - sy1) / (sx2 - sx1);
        sc = sy1 - (sm * sx1);
    }

    double ix = 0, iy = 0;

    if (isFVertical)
    {
        ix = fx1; // x value is either fx1 or fx2 because fx1 == fx2 since the line is vertical

        iy = (ix * sm) + sc; // use gradient and intercept of second line since ix will also be available on second line
    }
    else if (isSVertical)
    {
        ix = sx1;
        iy = (ix * fm) + fc;
    }
    else if (Math.Abs(sm - fm) < 1e-10) // 1e-10 => 1 * 10^-10
    {
        return null; // Gradients are almost the same so lines are parallel
    }
    else
    {
        ix = (sc - fc) / (fm - sm); // since they are both non vertical, at intersection point (x,y), m1x + c1 (for first line) = m2x + c2 (for second line)
        iy = (ix * fm) + fc;
    }

    //check if intersection point lies within each line given and not outside of it
    if (!isBetween(fx1, fx2, ix) || !isBetween(fy1, fy2, iy))
        return null;

    if (!isBetween(sx1, sx2, ix) || !isBetween(sy1, sy2, iy))
        return null;

    return new Point(ix, iy);
}

#endregion

#region Tic Tac Win

//Class is below for n x n board
TicTacToe obj = new TicTacToe(3);

//For 3 x 3 board
char TicTacToeWinner(char[][] board)
{
    // Check rows
    for (int r = 0; r < 3; r++)
    {
        if (board[r][0] != ' ' &&
            board[r][0] == board[r][1] &&
            board[r][1] == board[r][2])
        {
            return board[r][0];
        }
    }

    // Check columns
    for (int c = 0; c < 3; c++)
    {
        if (board[0][c] != ' ' &&
            board[0][c] == board[1][c] &&
            board[1][c] == board[2][c])
        {
            return board[0][c];
        }
    }

    // Check main diagonal
    if (board[0][0] != ' ' &&
        board[0][0] == board[1][1] &&
        board[1][1] == board[2][2])
    {
        return board[0][0];
    }

    // Check anti-diagonal
    if (board[0][2] != ' ' &&
        board[0][2] == board[1][1] &&
        board[1][1] == board[2][0])
    {
        return board[0][2];
    }

    // No winner
    return ' ';
}


#endregion

#region Factorial Zeros

int countTrailingZeros(int num)
{
    if (num < 0)
        return -1;

    var count = 0;

    for(int i = 5; num/i > 0; i *= 5)
    {
        count += num / i;
    }

    return count;
}

//Time O(Log5n)

#endregion

#region Smallest Difference

int getSmallestDiff(int[] A, int[] B)
{
    Array.Sort(A);
    Array.Sort(B);

    int i = 0, j = 0;
    var minDiff = int.MaxValue;

    while (i < A.Length && j < B.Length)
    {
        var diff = Math.Abs(A[i] - B[j]);

        minDiff = Math.Min(minDiff, diff);

        if (A[i] < B[j])
            i++;
        else
            j++;
    }

    return minDiff;
}

/*
 * Time: O(n log n + m log m) for sorting
 * Two-pointer scan: O(n + m)
 * Space: O(1)
*/

#endregion

#region Number Max

int NumberMax(int a, int b)
{
    var c = (long)a - (long)b; // prevent overflow; diff is negative if a is smaller and positive if a is bigger

    // In 64 bit numbers (long data type), the first from left (most significant) bit is a sign bit
    // and we right shift it to the least significant bit; after & with 1,  0 if a >= b, 1 if a < b
    var sign = (int)((c >> 63) & 1);

    var k = 1 - sign; // this operation in a way reverses the sign giving us either 0 or 1

    return a * k + b * sign; // multiplying the numbers with the reversed signs returns only one of them
}

#endregion

#region English Int

string NumberToWords(int num)
{
    if (num == 0)
        return "Zero";

    if (num < 0)
        return "Negative " + NumberToWords(-num);

    //193,235,123 as testcase
    var ones = new string[]{
            "", "One", "Two", "Three", "Four", "Five",
            "Six", "Seven", "Eight", "Nine"
        };

    var teens = new string[]{
            "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen",
            "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
        };

    var tens = new string[]{
            "", "", "Twenty", "Thirty", "Forty", "Fifty",
            "Sixty", "Seventy", "Eighty", "Ninety"
        };

    var thousands = new string[]{
            "", "Thousand", "Million", "Billion"
        };

    var result = new StringBuilder();
    var chunkIndex = 0;

    while (num > 0)
    {
        var chunk = num % 1000;

        if (chunk != 0)
        {
            var n = convertChunk(chunk) + thousands[chunkIndex] + " ";
            result.Insert(0, n);
        }

        chunkIndex++;
        num /= 1000;
    }

    string convertChunk(int num)
    {
        var res = new StringBuilder();

        if (num >= 100)
        {
            res.Append(ones[(num / 100)] + " Hundred ");
            num %= 100;
        }

        if (num >= 20)
        {
            var n = tens[num / 10] + " ";
            res.Append(n);
            num %= 10;
        }
        else if (num >= 10)
        {
            var n = teens[num - 10] + " ";
            res.Append(n);
            return res.ToString();
        }

        if (num > 0)
        {
            var n = ones[num] + " ";
            res.Append(n);
        }

        return res.ToString();
    }

    return result.ToString().Trim();
}

#endregion

#region Operations

int Negate(int num)
{
    var neg = 0;
    var delta = num < 0 ? 1 : -1;

    while(num != 0)
    {
        neg += delta;
        num += delta;
    }

    return neg;
}

int Subtract(int a, int b)
{
    return a + Negate(b);
}

int Multiplication(int a, int b)
{
    if(a == 0 || b == 0)
        return 0;

    var absA = Math.Abs(a);
    var absB = Math.Abs(b);

    var smaller = absA < absB ? absA : absB;
    var bigger = absA < absB ? absB : absA;

    var sum = 0;

    for(int i = 0; i < smaller; i++)
    {
        sum += bigger;
    }

    if((a < 0) ^ (b < 0))
        sum = Negate(sum);

    return sum;
}

int Division(int a, int b)
{
    if(b == 0)
        throw new DivideByZeroException();

    var absA = Math.Abs(a);
    var absB = Math.Abs(b);

    var quotient = 1;

    while (absA >= absB)
    {
        absA = Subtract(absA, absB);
        quotient += 1;
    }

    if ((a < 0) ^ (b < 0))
        quotient = Negate(quotient);

    return quotient;
}

#endregion

#region Living People

int GetYear(List<(int birth, int death)> people)
{
    var yearsPopulation = new int[102]; // index 0 = 1900, index 101 = 2001 sentinel 

    foreach(var(birth, death) in people)
    {
        yearsPopulation[birth - 1900] += 1;
        yearsPopulation[(death + 1) - 1900] += 1;
    }

    var maxYear = 1900;
    var maxPop = 0;

    var currentPop = 0;

    for(int i = 0; i < 101; i++) //101 is 2001 which is not required
    {
        currentPop += yearsPopulation[i];

        if(currentPop > maxPop)
        {
            maxPop = currentPop;
            maxYear = 1900 + i;
        }
    }

    return maxYear;
}

//Time: O(P + R)  → effectively O(P) since R is constant: 101
//Space: O(R)     → constant space ~ 101

#endregion

#region Diving Board

List<int> GetPossibleLengths(int shorter, int longer, int k)
{
    var result = new List<int>();

    if (k == 0)
        return result;

    if(shorter == longer)
    {
        result.Add(shorter * k);
        return result;
    }

    for(int i = 0; i <= k; i++)
    {
        var numLonger = i; // when number of longer planks is i, number of shorter planks will be k - i 
        var numShorter = k - i;

        var len = (numLonger * longer) + (numShorter * numShorter);
        result.Add(len);
    }

    return result;
}

#endregion

#region XML Encoding

string EncodeXML(XMLElement root, TagMapper map)
{
    var sb = new StringBuilder();

    EncodeXMLElement(root, map, sb);

    return sb.ToString().Trim();
}

void EncodeXMLElement(XMLElement ele, TagMapper map, StringBuilder sb)
{
    // 1. element tag id
    AppendToken(map.GetNum(ele.Tag).ToString(), sb);

    // 2. attributes: TagId Value 0
    foreach (var attr in ele.Attributes)
    {
        AppendToken(map.GetNum(attr.Tag).ToString(), sb);
        AppendToken(attr.Value, sb);
    }

    // 3. END of attributes
    AppendToken("0", sb);

    // 4. Children or value
    if (!string.IsNullOrEmpty(ele.Value))
    {
        AppendToken(ele.Value, sb);
    }
    else
    {
        foreach(var child in ele.Children)
        {
            EncodeXMLElement(child, map, sb);
        }
    }

    AppendToken("0", sb);
}

void AppendToken(string token, StringBuilder sb)
{
    sb.Append(token);
    sb.Append(' ');
}

#endregion

#endregion

class Point
{
    public double X { get; }
    public double Y { get; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
}

public class TicTacToe
{
    private int diagCount;
    private int antiDiagCount;
    private int[] rowsCount;
    private int[] colsCount;
    private int n;

    public TicTacToe(int n)
    {
        diagCount = 0;
        antiDiagCount = 0;
        rowsCount = new int[n];
        colsCount = new int[n];
        this.n = n;
    }

    public int Move(int row, int col, int player)
    {
        var addToPlayer = (player == 1) ? 1 : -1;

        rowsCount[row] += addToPlayer;
        colsCount[col] += addToPlayer;

        if (row == col)
            diagCount += addToPlayer;

        if ((row + col) == n - 1)
            antiDiagCount += addToPlayer;

        if (Math.Abs(rowsCount[row]) == n ||
           Math.Abs(colsCount[col]) == n ||
           Math.Abs(diagCount) == n ||
           Math.Abs(antiDiagCount) == n
           )
        {
            return player;
        }

        return 0;
    }
}

public class XMLAttribute
{
    public string Tag { get; set; }
    public string Value { get; set; }

    public XMLAttribute(string tag, string value)
    {
        Tag = tag;
        Value = value;
    }
}

public class XMLElement
{
    public string Tag { get; set; }
    public string Value { get; set; }
    public List<XMLAttribute> Attributes { get; } = new();
    public List<XMLElement> Children { get; } = new();

    public XMLElement(string tag)
    {
        Tag = tag;
    }
}

public class TagMapper
{
    private readonly Dictionary<string, int> _mapper;

    public TagMapper(Dictionary<string, int> mapper)
    {
        _mapper = mapper;
    }

    public int GetNum(string key)
    {
        return _mapper[key];
    }
}



