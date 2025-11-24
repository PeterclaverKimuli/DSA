using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public sealed class TimedKeyValueStore<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, LinkedListNode<Entry>> _map;
        private readonly LinkedList<Entry> _list = new();
        private readonly object _gate = new();

        private readonly TimeSpan _ttl;
        private readonly bool _isSlidingExpiration;
        private readonly int? _maxCapacity;

        private sealed class Entry { 
            public TKey Key { get; set; } = default!;
            public TValue Value { get; set; } = default!;
            public DateTimeOffset CreatedUtc { get; set; }
            public DateTimeOffset ExpiresUtc { get; set; }
        }

        /// <param name="ttl">Time to live for each entry.</param>
        /// <param name="slidingExpiration">If true, refresh expiration on access.</param>
        /// <param name="initialCapacity">Initial dictionary capacity (optional).</param>
        /// <param name="maxCapacity">Max items allowed; evicts oldest when exceeded (optional).</param>
        public TimedKeyValueStore(TimeSpan ttl, bool isSlidingExpiration = false, int intialCapacity = 0, int? maxCapacity = null)
        {
            if(ttl <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(ttl));
            if(maxCapacity is int mc && mc <= 0) throw new ArgumentOutOfRangeException(nameof(maxCapacity));

            _ttl = ttl;
            _isSlidingExpiration = isSlidingExpiration;
            _maxCapacity = maxCapacity;
            _map = new Dictionary<TKey, LinkedListNode<Entry>>(Math.Max(0, intialCapacity));
        }

        public int Count {
            get { lock (_gate) return _map.Count; }
        }

        /// <summary>Adds or updates an item. Returns true if added, false if updated.</summary>
        public bool AddOrUpdate(TKey key, TValue value, DateTimeOffset? nowUtc = null) {
            nowUtc ??= DateTimeOffset.UtcNow;

            lock (_gate) {
                if (_map.TryGetValue(key, out var node)) {
                    // Update existing
                    node.Value.Value = value;

                    // Refresh timestamps
                    node.Value.CreatedUtc = nowUtc.Value;
                    node.Value.ExpiresUtc = nowUtc.Value + _ttl;

                    // Move to tail (newest)
                    _list.Remove(node);
                    _map[key] = _list.AddLast(node.Value);
                    return false;
                }
                else
                {
                    // Insert new
                    var entry = new Entry
                    {
                        Key = key,
                        Value = value,
                        CreatedUtc = nowUtc.Value,
                        ExpiresUtc = nowUtc.Value + _ttl
                    };

                    var newNode = _list.AddLast(entry);
                    _map[key] = newNode;

                    if (_maxCapacity.HasValue && _map.Count > _maxCapacity.Value) { 
                        EvictHead_NoLock();
                    }

                    return true; // added
                }
            }
        }

        /// <summary>Try get value; respects expiration and sliding expiration.</summary>
        public bool TryGetValue(TKey key, out TValue value, DateTimeOffset? nowUtc = null)
        {
            nowUtc ??= DateTimeOffset.UtcNow;

            lock (_gate)
            {
                if(!_map.TryGetValue(key, out var node))
                {
                    value = default!;
                    return false;
                }

                if(node.Value.ExpiresUtc <= nowUtc.Value)
                {
                    // Expired: remove
                    RemoveNode_NoLock(node);
                    value = default!;
                    return false;
                }

                // Refresh for sliding expiration and move to tail (newest)
                if (_isSlidingExpiration) { 
                    node.Value.ExpiresUtc = nowUtc.Value + _ttl;
                }

                _list.Remove(node);
                _map[key] = _list.AddLast(node.Value);

                value = node.Value.Value;
                return true;
            }
        }

        /// <summary>Remove by key (returns true if found and removed).</summary>
        public bool Remove(TKey key) {
            lock (_gate) { 
                if(!_map.TryGetValue(key, out var node)) return false;

                RemoveNode_NoLock(node);
                return true;
            }
        }

        /// <summary>Purge all entries expired at or before 'nowUtc'. Returns number removed.</summary>
        public int PurgeExpired(DateTimeOffset? nowUtc = null) {
            nowUtc ??= DateTimeOffset.UtcNow;

            int removed = default;

            lock (_gate) {
                // Because we add at tail and keep oldest at head,
                // if TTL is fixed, expired items will accumulate at the head.
                while (_list.First is LinkedListNode<Entry> head && head.Value.ExpiresUtc <= nowUtc.Value) {
                    RemoveNode_NoLock(head);
                    removed++;
                }
            }

            return removed;
        }

        /// <summary>Clear everything.</summary>
        public void Clear() {
            lock (_gate) {
                _map.Clear();
                _list.Clear();
            }
        }

        // --------- helpers (callers must hold _gate) ---------
        private void EvictHead_NoLock() {
            //Check if _list.First is not null, assign it to var head and then remove it
            if (_list.First is { } head) { 
                RemoveNode_NoLock(head);
            }
        }

        private void RemoveNode_NoLock(LinkedListNode<Entry> node) {
            _list.Remove(node);
            _map.Remove(node.Value.Key);
        }
    }
}
