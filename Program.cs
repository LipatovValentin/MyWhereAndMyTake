using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyWhereAndMyTake
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in GetEnumiration().MyWhere(x => x % 2 == 0).Take(5))
            {
                Console.WriteLine(item);
            }
        }
        public static IEnumerable<int> GetEnumiration()
        {
            int i = 0;
            while (true)
            {
                yield return i++;
            }
        }
    }
    public class MyHelper<T> : IEnumerable<T>, IEnumerator<T>, IDisposable
    {
        protected bool _disposed = false;
        protected IEnumerator<T> _items;
        public T Current
        {
            get
            {
                ThrowIfDisposed();
                return _items.Current;
            }
        }
        object IEnumerator.Current
        {
            get
            {
                return (object) Current;
            }
        }
        public MyHelper(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            _items = items.GetEnumerator();
        }
        public virtual bool MoveNext()
        {
            ThrowIfDisposed();
            return _items.MoveNext();
        }
        public void Reset()
        {
            ThrowIfDisposed();
            _items.Reset();
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed == true)
            {
                throw new ObjectDisposedException(nameof(_items));
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        public void Dispose(bool disposing)
        {
            if (_disposed == true)
            {
                return;
            }
            if (disposing)
            {
                _items?.Dispose();
            }
            _disposed = true;
        }
        public MyHelper<T> GetEnumerator()
        {
            return this;
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
    public class MyWhenHelper<T> : MyHelper<T>
    {
        private Func<T, bool> _predicate;
        public MyWhenHelper(IEnumerable<T> items, Func<T, bool> predicate) : base (items)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            _predicate = predicate;
        }
        public override bool MoveNext()
        {
            ThrowIfDisposed();
            while (_items.MoveNext())
            {
                if (_predicate(Current) == true)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }
    public class MyTakeHelper<T> : MyHelper<T>
    {
        private int _counter;
        private int _length;
        public MyTakeHelper(IEnumerable<T> items, int length) : base(items)
        {
            _length = length;
        }
        public override bool MoveNext()
        {
            ThrowIfDisposed();
            while (_items.MoveNext())
            {
                if (_counter < _length)
                {
                    _counter = _counter + 1;
                    return true;
                }
                else
                {
                    break;
                }
            }
            return false;
        }
    }
    public static class Helper
    {
        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            return new MyWhenHelper<T>(items, predicate);
        }
        public static IEnumerable<T> MyTake<T>(this IEnumerable<T> items, int length)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return new MyTakeHelper<T>(items, length);
        }
    }
}
