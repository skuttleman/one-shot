using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Utils
{
    public static class Sequences
    {
        // void reduction
        public static void DoAll<T>(IEnumerable<T> coll) => ForEach(coll, _ => { });
        public static void ForEach<T>(IEnumerable<T> coll, Action<T> action)
        {
            if (coll != null) foreach (T item in coll) action(item);
        }

        // value reduction
        public static U Reduce<T, U>(IEnumerable<T> coll, Func<U, T, U> reducer, U init)
        {
            U result = init;
            if (coll != null)
            {
                foreach (T item in coll) result = reducer(result, item);
            }
            return result;
        }
        public static U ReduceUntil<T, U>(IEnumerable<T> coll, Func<U, T, Reduction<U>> reducer, U init)
        {
            U result = init;
            if (coll != null)
            {
                foreach (T item in coll)
                {
                    Reduction<U> next = reducer(result, item);
                    result = next.Get();
                    if (next.IsReduced()) return result;
                }
            }
            return result;
        }

        // sequence transposition
        public static Sequence<T> Rest<T>(IEnumerable<T> coll) =>
            Sequence<T>.Of(RestEnumerable(coll.GetEnumerator()));
        public static Sequence<U> MapCat<T, U>(IEnumerable<T> coll, Func<T, IEnumerable<U>> fn) =>
            Sequence<U>.Of(Expand(coll, fn));
        public static Sequence<U> Map<T, U>(IEnumerable<T> coll, Func<T, U> fn) =>
            Sequence<U>.Of(Expand(coll, item => new U[] { fn(item) }));
        public static Sequence<V> Map<T, U, V>(
            IEnumerable<T> coll1,
            IEnumerable<U> coll2,
            Func<T, U, V> fn)
        {
            return Sequence<V>.Of(Expand(
                coll1,
                coll2,
                (item1, item2) => new V[] { fn(item1, item2) }));

        }
        public static Sequence<T> Filter<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Sequence<T>.Of(Expand(
                coll,
                item => pred(item) ? new T[] { item } : new T[] { }));
        public static Sequence<T> Remove<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Filter(coll, Fns.Compliment(pred));
        public static Sequence<T> Take<T>(IEnumerable<T> coll, long n)
        {
            return
                Map(
                    Limit(
                        Map(Colls.Range(), coll, (idx, item) => (idx, item)),
                        t => t.idx < n),
                    t => t.item);
        }
        public static Sequence<T> Drop<T>(IEnumerable<T> coll, long n) =>
            n > 0 ? Drop(Rest(coll), n - 1) : Sequence<T>.Of(coll);

        // generation
        public static Sequence<T> Iterate<T>(T init, Func<T, T> nextFn) =>
            Sequence<T>.Of(Iterator(init, nextFn));

        // extraction
        public static T Find<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Sequence<T>.Of(coll).Filter(pred).First();
        public static T Find<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Find(coll, Colls.Pred(dict));
        public static T Find<T>(IEnumerable<T> coll, ISet<T> set) =>
            Find(coll, Colls.Pred(set));
        public static T First<T>(IEnumerable<T> coll)
        {
            if (coll != null) foreach (T item in coll) return item;
            return default;
        }

        // internal
        private static IEnumerable<T> RestEnumerable<T>(IEnumerator<T> enumerator)
        {
            if (enumerator.MoveNext())
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
        }
        private static IEnumerable<U> Expand<T, U>(IEnumerable<T> coll, Func<T, IEnumerable<U>> expander)
        {
            if (coll != null)
                foreach (T input in coll)
                    foreach (U item in expander(input))
                        yield return item;
        }
        private static IEnumerable<V> Expand<T, U, V>(
            IEnumerable<T> coll1,
            IEnumerable<U> coll2,
            Func<T, U, IEnumerable<V>> expander)
        {
            if (coll1 != null)
            {
                if (coll2 != null)
                {
                    IEnumerator<T> iter1 = coll1.GetEnumerator();
                    IEnumerator<U> iter2 = coll2.GetEnumerator();

                    while (true)
                    {
                        T val1;
                        U val2;

                        if (iter1.MoveNext())
                        {
                            val1 = iter1.Current;
                            if (iter2.MoveNext())
                            {
                                val2 = iter2.Current;
                                foreach (V item in expander(val1, val2))
                                    yield return item;
                            }
                            else break;
                        }
                        else break;
                    }
                }
            }
        }
        private static IEnumerable<T> Iterator<T>(T value, Func<T, T> nextFn)
        {
            while (true)
            {
                yield return value;
                value = nextFn(value);
            }
        }
        private static IEnumerable<T> Limit<T>(IEnumerable<T> coll, Predicate<T> pred)
        {
            if (coll != null)
            {
                foreach (T item in coll)
                {
                    if (pred(item)) yield return item;
                    else break;
                }
            }
        }
    }

    public class Sequence<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> coll;

        public static Sequence<T> Of(IEnumerable<T> coll) => new(coll);

        public T First() => Sequences.First(coll);
        public Sequence<T> Rest() => Sequences.Rest(coll);
        public U Reduce<U>(Func<U, T, U> reducer, U init) =>
            Sequences.Reduce(coll, reducer, init);
        public U ReduceUntil<U>(Func<U, T, Reduction<U>> reducer, U init) =>
            Sequences.ReduceUntil(coll, reducer, init);
        public Sequence<T> Take(long n) => Sequences.Take(coll, n);
        public void ForEach(Action<T> action) => Sequences.ForEach(coll, action);
        public Sequence<U> MapCat<U>(Func<T, IEnumerable<U>> fn) =>
            Sequences.MapCat(coll, fn);
        public Sequence<U> Map<U>(Func<T, U> fn) => Sequences.Map(coll, fn);
        public Sequence<V> Map<U, V>(IEnumerable<U> coll2, Func<T, U, V> fn) =>
            Sequences.Map(coll, coll2, fn);
        public Sequence<T> Filter(Predicate<T> pred) =>
            Sequences.Filter(coll, pred);
        public Sequence<T> Remove(Predicate<T> pred) =>
            Sequences.Filter(coll, Fns.Compliment(pred));

        public IEnumerator<T> GetEnumerator() => coll.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        private Sequence(IEnumerable<T> coll) => this.coll = coll;
    }

    public class Reduction<T>
    {
        readonly T item;
        readonly bool isReduced;

        private Reduction(T item, bool isReduced)
        {
            this.item = item;
            this.isReduced = isReduced;
        }

        public static Reduction<T> Reduced(T item) => new(item, true);
        public static Reduction<T> UnReduced(T item) => new(item, false);
        public bool IsReduced() => isReduced;
        public T Get() => item;
    }
}
