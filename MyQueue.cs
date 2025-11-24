using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class MyQueue<T>
    {
        private class QueueNode<T>
        {
            public T data;
            public QueueNode<T> next;

            public QueueNode(T data)
            {
                this.data = data;
            }
        }

        private QueueNode<T> first;
        private QueueNode<T> last;

        public void Add(T data) {
            QueueNode<T> item = new QueueNode<T>(data);

            if (last != null) { 
                last.next = item;
            }

            last = item;

            if (first == null) { 
                first = last;
            }
        }

        public T Remove()
        {
            if (first == null) throw new NullReferenceException();

            T data = first.data;
            first = first.next;

            if (first == null) last = null;

            return data;
        }

        public T Peek() {
            if (first == null) throw new NullReferenceException();

            return first.data;
        }

        public bool IsEmpty() { 
            return first == null;
        }
    }

    public class MyQueueUsingStacks<T>
    {
        Stack<T> newItems = new Stack<T>();
        Stack<T> oldItems = new Stack<T>();

        public void Add(T item) { 
            newItems.Push(item);
        }

        public int Count => newItems.Count + oldItems.Count;

        public T Remove() { 
            ShiftStacks();

            if(oldItems.Count == 0) throw new InvalidOperationException("Queue is Empty");

            return oldItems.Pop();
        }

        public T Peek() {
            ShiftStacks();

            if (oldItems.Count == 0) throw new InvalidOperationException("Queue is Empty");

            return oldItems.Peek();
        }

        public bool IsEmpty() => Count == 0; 

        private void ShiftStacks()
        {
            if(oldItems.Count == 0)
            {
                while(newItems.Count > 0)
                {
                    oldItems.Push(newItems.Pop());
                }
            }
        }
    }

    #region Animal Shelter
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Order { get; set; }

        protected Animal(string name) { 
            this.Name = name;
        }

        public bool IsOlderThan(Animal other) => this.Order < other.Order;
    }

    public class Dog : Animal
    {
        public Dog(string name) : base(name) { }
    }

    public class Cat : Animal
    {
        public Cat(string name) : base(name) { }
    }

    public class AnimalShelterQueue
    {
        private LinkedList<Dog> dogs = new LinkedList<Dog>();
        private LinkedList<Cat> cats = new LinkedList<Cat>();
        private int order = 0;

        public void Enqueue(Animal animal)
        {
            animal.Order = order++;

            if (animal is Dog dog)
                dogs.AddLast(dog);
            else if(animal is Cat cat)
                cats.AddLast(cat);
        }

        public Animal DequeueAny()
        {
            if(dogs.Count == 0) return DequeueCat();
            if(cats.Count == 0) return DequeueDog();

            Dog eldestDog = dogs.First.Value;
            Cat eldestCat = cats.First.Value;

            if (eldestDog.IsOlderThan(eldestCat))
                return DequeueDog();
            else
                DequeueCat();

            throw new InvalidOperationException("No animals to Dequeue");
        }

        public Dog DequeueDog()
        {
            if (dogs.Count == 0) return null;

            var dog = dogs.First.Value;
            dogs.RemoveFirst();

            return dog;
        }

        public Cat DequeueCat()
        {
            if (cats.Count == 0) return null;

            var cat = cats.First.Value;
            cats.RemoveFirst();

            return cat;
        }
    }
    #endregion
}
