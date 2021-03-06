using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWhereAndMyTake
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            foreach (var item in GetEnumiration().Where(x => x % 2 == 0).Take(5))
            {
                Console.WriteLine(item);
            }
            */

            /*
            foreach (var item in GetEnumiration().MyTake(5).MyWhere(x => x % 2 == 0))
            {
                Console.WriteLine(item);
            }
            */

            var array = Enumerable.Range(0, 25);
            foreach (var item in array.MyWhere(x => x % 2 == 0).MyTake(5))
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
    public static class Helper
    {
        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> collectionIEnumerable, Func<T, bool> function)
        {
            IEnumerable<T> collection = new T[0];
            if (collectionIEnumerable == null)
            {
                throw new ArgumentNullException(nameof(collectionIEnumerable));
            }
            else
            {
                IEnumerator<T> collectionIEnumerator = collectionIEnumerable.GetEnumerator();
                while (collectionIEnumerator.MoveNext())
                {
                    T item = (T)collectionIEnumerator.Current;
                    if (function(item) == true)
                    {
                        collection = collection.MyAdd(item);
                        // yield return item;
                    }
                }
                return (IEnumerable<T>)collection;
            }
        }
        public static IEnumerable<T> MyTake<T>(this IEnumerable<T> collectionIEnumerable, int length)
        {
            IEnumerable<T> collection = new T[0];
            if (collectionIEnumerable == null)
            {
                throw new ArgumentNullException(nameof(collectionIEnumerable));
            }
            else
            {
                IEnumerator<T> collectionIEnumerator = collectionIEnumerable.GetEnumerator();
                int counter = 0;
                while (collectionIEnumerator.MoveNext())
                {
                    T item = (T)collectionIEnumerator.Current;
                    if (counter < length)
                    {
                        collection = collection.MyAdd(item);
                        // yield return item;
                        counter = counter + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                return (IEnumerable<T>)collection;
            }
        }
        public static int MyLength<T>(this IEnumerable<T> collectionIEnumerable)
        {
            if (collectionIEnumerable == null)
            {
                throw new ArgumentNullException(nameof(collectionIEnumerable));
            }
            else
            {
                IEnumerator<T> collectionIEnumerator = collectionIEnumerable.GetEnumerator();
                int counter = 0;
                while (collectionIEnumerator.MoveNext())
                {
                    counter = counter + 1;
                }
                return counter;
            }
        }
        public static IEnumerable<T> MyAdd<T>(this IEnumerable<T> collectionIEnumerable, T value)
        {
            if (collectionIEnumerable == null)
            {
                throw new ArgumentNullException(nameof(collectionIEnumerable));
            }
            else
            {
                IEnumerator<T> collectionIEnumerator = collectionIEnumerable.GetEnumerator();
                int length = collectionIEnumerable.MyLength<T>();
                T[] collection = new T[length + 1];
                int counter = 0;
                while (collectionIEnumerator.MoveNext())
                {
                    T item = (T)collectionIEnumerator.Current;
                    collection[counter] = item;
                    counter = counter + 1;
                }
                collection[length] = value;
                return (IEnumerable<T>)collection;
            }
        }
    }
}
