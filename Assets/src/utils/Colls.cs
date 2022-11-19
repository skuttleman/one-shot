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

        public static U Reduce<T, U>(IEnumerable<T> coll, Func<U, T, U> reducer, U init) =>
            Sequence<T>.Of(coll).Reduce(reducer, init);

        public static U ReduceUntil<T, U>(IEnumerable<T> coll, Func<U, T, Reduction<U>> reducer, U init) =>
            Sequence<T>.Of(coll).ReduceUntil(reducer, init);
        public static void DoAll<T>(IEnumerable<T> coll) => ForEach(coll, _ => { });
        public static IEnumerable<U> MapCat<T, U>(IEnumerable<T> coll, Func<T, IEnumerable<U>> fn) =>
            Sequence<T>.Of(coll).MapCat(fn);
        public static IEnumerable<U> Map<T, U>(IEnumerable<T> coll, Func<T, U> fn) =>
            Sequence<T>.Of(coll).Map(fn);
        public static IEnumerable<U> Map<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Map(coll, Fn(dict));
        public static IEnumerable<T> Filter<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Sequence<T>.Of(coll).Filter(pred);
        public static IEnumerable<T> Filter<T>(IEnumerable<T> coll, ISet<T> set) =>
            Filter(coll, Pred(set));
        public static IEnumerable<T> Filter<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Filter(coll, Pred(dict));
        public static T Find<T>(IEnumerable<T> coll, Predicate<T> pred) =>
            Sequence<T>.Of(coll).Filter(pred).First();
        public static T Find<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Find(coll, Pred(dict));
        public static T Find<T>(IEnumerable<T> coll, ISet<T> set) =>
            Find(coll, Pred(set));
        public static T First<T>(IEnumerable<T> coll) => Sequence<T>.Of(coll).First();
        public static IEnumerable<T> Rest<T>(IEnumerable<T> coll) =>
            Sequence<T>.Of(coll).Rest();
        public static IEnumerable<T> Take<T>(IEnumerable<T> coll, long n) =>
            Sequence<T>.Of(coll).Take(n);
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
            Iterator<long>.Of(0, x => x + 1);
        public static IEnumerable<long> Range(long end) => Range(0, end);
        public static IEnumerable<long> Range(long start, long end) =>
            Limiter<long>.Of(Iterator<long>.Of(start, x => x + 1), x => x < end);
    }

    public class Sequence<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> coll;

        public static Sequence<T> Of(IEnumerable<T> coll) => new(coll);

        public T First()
        {
            if (coll != null) foreach (T item in coll) return item;
            return default;
        }

        public Sequence<T> Rest()
        {
            if (coll != null)
            {
                IEnumerator<T> enumerator = coll.GetEnumerator();
                if (enumerator.MoveNext()) return new(Enumerator<T>.Of(enumerator));
            }
            return new(new T[] { });
        }

        public U Reduce<U>(Func<U, T, U> reducer, U init)
        {
            U result = init;
            if (coll != null)
            {
                foreach (T item in coll) result = reducer(result, item);
            }
            return result;
        }

        public U ReduceUntil<U>(Func<U, T, Reduction<U>> reducer, U init)
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

        public Sequence<T> Take(long n)
        {
            long index = 0;
            return
                Limiter<(long, T)>.Of(
                    Map(x => (index++, x)),
                    x => x.Item1 < n)
                .Map<T>(x => x.Item2);
        }

        public void ForEach(Action<T> action)
        {
            foreach (T item in coll) action(item);
        }

        public Sequence<U> MapCat<U>(Func<T, IEnumerable<U>> fn) =>
            new(Expander<T, U>.Of(coll, fn));
        public Sequence<U> Map<U>(Func<T, U> fn) =>
            new(Expander<T, U>.Of(coll, input => new U[] { fn(input) }));
        public Sequence<T> Filter(Predicate<T> pred) =>
            new(Expander<T, T>.Of(
                    coll,
                    item => pred(item) ? new T[] { item } : new T[] { }));

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

    public class Limiter<T> : IEnumerable<T>
    {
        readonly IEnumerable<T> coll;
        readonly Predicate<T> pred;

        public static Sequence<T> Of(IEnumerable<T> coll, Predicate<T> pred) =>
            Sequence<T>.Of(new Limiter<T>(coll, pred));

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

        private Limiter(IEnumerable<T> coll, Predicate<T> pred)
        {
            this.coll = coll;
            this.pred = pred;
        }
    }

    public class Expander<T, U> : IEnumerable<U>
    {
        readonly IEnumerable<T> coll;
        readonly Func<T, IEnumerable<U>> nextFn;

        public static Sequence<U> Of(IEnumerable<T> coll, Func<T, IEnumerable<U>> nextFn) =>
            Sequence<U>.Of(new Expander<T, U>(coll, nextFn));

        public IEnumerator<U> GetEnumerator()
        {
            if (coll != null)
                foreach (T input in coll)
                    foreach (U item in nextFn(input))
                        yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Expander(IEnumerable<T> coll, Func<T, IEnumerable<U>> nextFn)
        {
            this.coll = coll;
            this.nextFn = nextFn;
        }
    }

    public class Enumerator<T> : IEnumerable<T>
    {
        IEnumerator<T> enumerator;

        public static Sequence<T> Of(IEnumerator<T> enumerator) =>
            Sequence<T>.Of(new Enumerator<T>(enumerator));

        public IEnumerator<T> GetEnumerator() => enumerator;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        private Enumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;
    }

    public class Iterator<T> : IEnumerable<T>
    {
        T value;
        readonly Func<T, T> nextFn;

        public static Sequence<T> Of(T init, Func<T, T> nextFn) =>
            Sequence<T>.Of(new Iterator<T>(init, nextFn));

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                yield return value;
                value = nextFn(value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Iterator(T init, Func<T, T> nextFn)
        {
            value = init;
            this.nextFn = nextFn;
        }
    }
}