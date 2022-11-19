using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Utils
{
    public static class Colls
    {
        public static void ForEach<T>(IEnumerable<T> coll, Action<T> action)
        {
            if (coll != null) foreach (T item in coll) action(item);
        }

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

        public static IEnumerable<U> MapCat<T, U>(IEnumerable<T> coll, Func<T, IEnumerable<U>> fn) =>
            new Expander<T, U>(coll, fn);

        public static IEnumerable<U> Map<T, U>(IEnumerable<T> coll, Func<T, U> fn) =>
            new Expander<T, U>(coll, input => new U[] { fn(input) });

        public static IEnumerable<U> Map<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Map(coll, Fn(dict));

        public static IEnumerable<T> Filter<T>(IEnumerable<T> coll, Predicate<T> pred)
        {
            return new Expander<T, T>(
                coll,
                item => pred(item) ? new T[] { item } : new T[] { });
        }

        public static IEnumerable<T> Filter<T>(IEnumerable<T> coll, ISet<T> set) =>
            Filter(coll, Pred(set));
        public static IEnumerable<T> Filter<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Filter(coll, Pred(dict));
        public static T Find<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Find(coll, pred, default);
        public static T Find<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Find(coll, Pred(dict));
        public static T Find<T>(IEnumerable<T> coll, ISet<T> set) =>
            Find(coll, Pred(set));

        public static T First<T>(IEnumerable<T> coll)
        {
            if (coll != null) foreach (T item in coll) return item;
            return default;
        }

        public static IEnumerable<T> Rest<T>(IEnumerable<T> coll)
        {
            if (coll != null)
            {
                IEnumerator<T> enumerator = coll.GetEnumerator();
                if (enumerator.MoveNext()) return new Enumerator<T>(enumerator);
            }
            return new T[] { };
        }

        public static IEnumerable<T> Take<T>(IEnumerable<T> coll, long n)
        {
            long index = 0;
            return Pipeline<IEnumerable<T>>.Of(coll)
                .Chain(coll => Map(coll, x => ValueTuple.Create(index++, x)))
                .Chain(coll => new Limiter<ValueTuple<long, T>>(coll, x => x.Item1 < n))
                .Chain(coll => Map(coll, x => x.Item2))
                .Extract();
        }

        public static T Find<T>(IEnumerable<T> coll, Predicate<T> pred, T otherwise)
        {
            return ReduceUntil(
                coll,
                (u, t) => pred(t) ? Reduction<T>.Reduced(t) : Reduction<T>.UnReduced(u),
                otherwise);
        }

        public static IEnumerable<T> Remove<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Filter(coll, Fns.Compliment(pred));

        public static L Add<L, T>(L coll, T item) where L : ICollection<T>
        {
            coll.Add(item);
            return coll;
        }

        public static Func<T, U> Fn<T, U>(IDictionary<T, U> dict) =>
            item => dict.ContainsKey(item) ? dict[item] : default;
        public static Predicate<T> Pred<T, U>(IDictionary<T, U> dict) =>
            item => dict.ContainsKey(item);
        public static Predicate<T> Pred<T>(ISet<T> set) =>
            item => set.Contains(item);

        public static U Get<T, U>(IDictionary<T, U> dict, T key)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return default;
        }

        public static IEnumerable<long> Range() =>
            new Iterator<long>(0, x => x + 1);
        public static IEnumerable<long> Range(long end) => Range(0, end);
        public static IEnumerable<long> Range(long start, long end) =>
            new Limiter<long>(new Iterator<long>(start, x => x + 1), x => x < end);

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

    public class Limiter<T> : IEnumerable<T>
    {
        readonly IEnumerable<T> coll;
        readonly Predicate<T> pred;

        public Limiter(IEnumerable<T> coll, Predicate<T> pred)
        {
            this.coll = coll;
            this.pred = pred;
        }

        public IEnumerator<T> GetEnumerator()
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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Expander<T, U> : IEnumerable<U>
    {
        readonly IEnumerable<T> coll;
        readonly Func<T, IEnumerable<U>> nextFn;

        public Expander(IEnumerable<T> coll, Func<T, IEnumerable<U>> nextFn)
        {
            this.coll = coll;
            this.nextFn = nextFn;
        }

        public IEnumerator<U> GetEnumerator()
        {
            if (coll != null)
                foreach (T input in coll)
                    foreach (U item in nextFn(input))
                        yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Enumerator<T> : IEnumerable<T>
    {
        IEnumerator<T> enumerator;

        public Enumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;

        public IEnumerator<T> GetEnumerator() => enumerator;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Iterator<T> : IEnumerable<T>
    {
        T value;
        readonly Func<T, T> nextFn;

        public Iterator(T init, Func<T, T> nextFn)
        {
            value = init;
            this.nextFn = nextFn;
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                yield return value;
                value = nextFn(value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Pipeline<T>
    {
        readonly T item;

        private Pipeline(T item) => this.item = item;

        public static Pipeline<T> Of(T item) => new(item);

        public T Extract() => item;

        public Pipeline<R> Chain<R>(Func<T, R> fn) => new(fn(item));
        public Pipeline<T> Doto(Action<T> action) { action(item); return this; }
    }

}