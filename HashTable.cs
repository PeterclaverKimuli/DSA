using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class HashEntry<K, V>
    {
        public K Key { get; set; }
        public V Value {  get; set; }

        public HashEntry(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }

    public class HashTable<K, V>
    {
        private readonly int _size;
        private readonly List<HashEntry<K, V>>[] buckets;

        public HashTable(int size)
        {
            _size = size;
            buckets = new List<HashEntry<K, V>>[size];

            for (int i = 0; i < buckets.Length; i++)
                buckets[i] = new List<HashEntry<K, V>>();
        }

        private int GetHashIndex(K key)
        {
            var hash = key.GetHashCode();
            return Math.Abs(hash % _size);
        }

        private List<HashEntry<K, V>> GetBucket(K key)
        {
            var index = GetHashIndex(key);
            return buckets[index];
        }

        public void Add(K key, V value)
        {
            var bucket = GetBucket(key);

            foreach(var entry in bucket)
            {
                if (entry.Key.Equals(key))
                {
                    entry.Value = value;
                    return;
                }
            }

            bucket.Add(new HashEntry<K, V>(key, value));
        }

        public V Get(K key)
        {
            var bucket = GetBucket(key);

            foreach (var hashEntry in bucket)
            {
                if (hashEntry.Key.Equals(key))
                    return hashEntry.Value;
            }

            throw new KeyNotFoundException("Key not found.");
        }

        public bool Remove(K key)
        {
            var bucket = GetBucket(key);

            for(int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    bucket.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void PrintAll()
        {
            for(int i = 0; i < _size; i++)
            {
                Console.WriteLine($"Bucket[{i}]");
                foreach(var hashEntry in buckets[i])
                {
                    Console.WriteLine($"{hashEntry.Key} : {hashEntry.Value}");
                }

                Console.WriteLine();
            }
        }
    }
}
