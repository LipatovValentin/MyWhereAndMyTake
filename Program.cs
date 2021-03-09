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
            Console.ReadKey();
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
        protected IEnumerator<T> _items;
        public T Current
        {
            get
            {
                return this._items.Current;
            }
        }
        object IEnumerator.Current
        {
            get
            {
                return (object) this.Current;
            }
        }
        public MyHelper(IEnumerable<T> items)
        {
            this._items = items.GetEnumerator();
        }
        public virtual bool MoveNext()
        {
            return this._items.MoveNext();
        }
        public void Reset()
        {
            this._items.Reset();
        }
        public void Dispose()
        {
            this._items.Dispose();
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
            this._predicate = predicate;
        }
        public override bool MoveNext()
        {
            while (this._items.MoveNext())
            {
                if (this._predicate(this.Current) == true)
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
            this._length = length;
        }
        public override bool MoveNext()
        {
            while (this._items.MoveNext())
            {
                if (this._counter < this._length)
                {
                    this._counter = this._counter + 1;
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
        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> collectionIEnumerable, Func<T, bool> predicate)
        {
            return new MyWhenHelper<T>(collectionIEnumerable, predicate);
        }
        public static IEnumerable<T> MyTake<T>(this IEnumerable<T> collectionIEnumerable, int length)
        {
            return new MyTakeHelper<T>(collectionIEnumerable, length);
        }
    }
}
