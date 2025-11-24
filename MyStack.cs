using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class MyStack<T>
    {
        private class StackNode<T>
        {
            public T data;
            public StackNode<T> next;

            public StackNode(T data)
            {
                this.data = data;
            }

        }

        private StackNode<T> top;

        public T pop()
        {
            if(top == null) throw new NullReferenceException();

            T item = top.data;
            top = top.next;

            return item;
        }

        public void push(T item)
        {
            StackNode<T> t = new StackNode<T>(item);
            t.next = top;
            top = t;
        }

        public T peek()
        {
            if (top == null) throw new NullReferenceException();
            return top.data;
        }

        public bool isEmpty()
        {
            return top == null;
        }
    }

    public class OneArrayThreeStacks
    {
        private int numberOfStacks = 3;
        private int eachStackCapacity;
        private int[] values;
        private int[] stackSizes;

        public OneArrayThreeStacks(int eachStackQuantity)
        {
            eachStackCapacity = eachStackQuantity;
            values = new int[numberOfStacks * eachStackQuantity]; // All values of three stacks in one array
            stackSizes = new int[numberOfStacks]; // Tracks current size of each stack
        }

        // Push a value onto stackNum (0, 1, or 2)
        public void Push(int stackNum, int value)
        {
            if (isStackFull(stackNum)) throw new InvalidOperationException("Stack is Full");

            stackSizes[stackNum]++;
            values[indexOfTop(stackNum)] = value;
        }

        // Pop the top value from stackNum
        public int Pop(int stackNum)
        {
            if (isStackEmpty(stackNum)) throw new InvalidOperationException("Stack is Empty");

            int value = values[indexOfTop(stackNum)];
            values[indexOfTop(stackNum)] = 0; // Optional: clear
            stackSizes[stackNum]--;

            return value;
        }

        public int Peek(int stackNum)
        {
            if (isStackEmpty(stackNum)) throw new InvalidOperationException("Stack is Empty");

            return values[indexOfTop(stackNum)];
        }

        public bool isStackFull(int stackNum) => stackSizes[stackNum] == eachStackCapacity;
        public bool isStackEmpty(int stackNum) => stackSizes[stackNum] == 0;

        private int indexOfTop(int stackNum)
        {
            int offset = stackNum * eachStackCapacity;
            int size = stackSizes[stackNum];
            return offset + size - 1;
        }
    }

    public class MinStack
    {
        private Stack<int> mainStack = new Stack<int>();
        private Stack<int> minStack = new Stack<int>();

        public void Push(int value)
        {
            mainStack.Push(value);

            if (minStack.Count == 0 || value <= minStack.Peek())
                minStack.Push(value);
        }

        public int Pop()
        {
            if (mainStack.Count == 0) throw new InvalidOperationException("Stack is Empty");

            int value = mainStack.Pop();

            if (value == minStack.Peek())
                minStack.Pop();

            return value;
        }

        public int Min()
        {
            if (minStack.Count == 0) throw new InvalidOperationException("Stack is Empty");

            return minStack.Peek();
        }
    }

    public class SetOfStacks
    {
        private readonly int eachStackCapacity;
        private List<Stack<int>> stacks;

        public SetOfStacks(int stackCapacity)
        {
            if (stackCapacity <= 0) throw new InvalidOperationException("Capacity must be greater than 0");

            eachStackCapacity = stackCapacity;
            stacks = new List<Stack<int>>();
        }

        public void Push(int value)
        {
            // ^1 means the last element in the list. ^2 would mean second element from last
            if (stacks.Count == 0 || stacks[^1].Count == eachStackCapacity) stacks.Add(new Stack<int>());

            stacks[^1].Push(value);
        }

        public int Pop()
        {
            if (stacks.Count == 0) throw new InvalidOperationException("Stack is Empty");

            var last = stacks[^1];
            int value = last.Pop();

            if (last.Count == 0) stacks.RemoveAt(stacks.Count - 1);

            return value;
        }

        public int PopAt(int index)
        {
            if (index < 0 || index >= stacks.Count) throw new IndexOutOfRangeException();

            int value = PopWithShift(index);
            return value;
        }

        private int PopWithShift(int index)
        {
            var stack = stacks[index];
            var value = stack.Pop();

            for (int i = index; i < stacks.Count - 1; i++)
            {
                var bottom = RemoveBottom(stacks[i + 1]);

                stacks[i].Push(bottom);
            }

            // Clean up the last stack if it's empty
            if (stacks[^1].Count == 0) stacks.RemoveAt(stacks.Count - 1);

            return value;
        }

        private int RemoveBottom(Stack<int> stack)
        {
            var temp = new Stack<int>();

            while (stack.Count > 1)
            {
                temp.Push(stack.Pop());
            }

            var bottom = stack.Pop();

            while (temp.Count > 0)
            {
                stack.Push(temp.Pop());
            }

            return bottom;
        }
    }
}
