﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheLib
{
    public class Cache<TKey,TValue>:IStorage<TKey,TValue>
    {
        private readonly Dictionary<TKey, CacheValue> _dictionary;
        public int Size { get; private set; }
        public int Count { get; private set; }
        public int AbsoluteExpiration { get; private set; }
        public int SlidingExpiration { get; private set; }
        private readonly IStorage<TKey, TValue> _storage;
        private readonly IDateTimeProvider _dateTime;

        private class CacheValue
        {
            public TValue Value { get; private set; }
            public DateTime CreationTime { get; private set; }
            public DateTime LastAccess { get; set; }
            public CacheValue(TValue value,IDateTimeProvider dateTime)
            {
                CreationTime = dateTime.GetTime();
                LastAccess = dateTime.GetTime();
                Value = value;
            }
        }
		
		//slidingexpiration -> slidingExpiration
		//absoluteexpiration -> absoluteExpiration
        public Cache(int size, int slidingexpiration, int absoluteexpiration, IStorage<TKey, TValue> storage, IDateTimeProvider dateTime)
        {
            Size = size;
            AbsoluteExpiration = absoluteexpiration;
            SlidingExpiration = slidingexpiration;
            Count = 0;
            _dictionary = new Dictionary<TKey, CacheValue>(size);
            _storage = storage;
            _dateTime = dateTime;
        }

        private void Add(TKey key, TValue value)
        {
            RemoveExpired();
            var data = new CacheValue(value,_dateTime);
            if (!Exist(key))
            {
                if (Size <= Count)
                {
                    RemoveOldest();
                }
            }
            Count++;
            _dictionary[key] = data;
        }

        private void Remove(TKey key)
        {
            _dictionary.Remove(key);
            Count--;
        }

        private bool Exist(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        private void RemoveOldest()
        {
            var min = _dictionary.First().Value.LastAccess;
            var minkey = _dictionary.First().Key;
            foreach (var key in _dictionary.Keys)
            {
                if (_dictionary[key].LastAccess < min)
                {
                    min = _dictionary[key].LastAccess;
                    minkey = key;
                }
            }
            Remove(minkey);
        }

        private void RemoveExpired()
        {
            var time = _dateTime.GetTime();
            var keys = new TKey[_dictionary.Keys.Count];
            _dictionary.Keys.CopyTo(keys,0);
            foreach (var key in keys)
            {
                if (_dictionary[key].CreationTime.AddMilliseconds(AbsoluteExpiration) < time || _dictionary[key].LastAccess.AddMilliseconds(SlidingExpiration) < time)
                {
                    Remove(key);
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_dictionary.ContainsKey(key))
                {
                    _dictionary[key].LastAccess = _dateTime.GetTime();
                }
                else
                {
                    Add(key, _storage.Get(key));
                }
                return _dictionary[key].Value;
            }
        }

        public TValue Get(TKey key)
        {
            return this[key];
        }
    }
}
