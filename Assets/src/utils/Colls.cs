using System;
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

        public static ICollection<U> Map<T, U>(IEnumerable<T> coll, Func<T, U> fn)
        {
            return Reduce(
                coll,
                (acc, item) => { acc.Add(fn(item)); return acc; },
                new List<U>());
        }

        public static ICollection<U> Map<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
            Map(coll, Fn(dict));

        public static ICollection<T> Filter<T>(IEnumerable<T> coll, Predicate<T> pred)
        {
            return Reduce(
                coll,
                (acc, item) => pred(item) ? Add(acc, item) : acc,
                new List<T>());
        }

        public static ICollection<T> Filter<T>(IEnumerable<T> coll, ISet<T> set) =>
            Filter(coll, Pred(set));
        public static ICollection<T> Filter<T, U>(IEnumerable<T> coll, IDictionary<T, U> dict) =>
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
                IList<T> result = new List<T>();
                bool toBeReturned = false;
                foreach(T item in coll)
                {
                    if (toBeReturned) result.Add(item);
                    toBeReturned = true;
                }
                if (result.Count > 0) return result;
            }
            return null;
        }

        public static T Find<T>(IEnumerable<T> coll, Predicate<T> pred, T otherwise)
        {
            return ReduceUntil(
                coll,
                (u, t) => pred(t) ? Reduction<T>.Reduced(t) : Reduction<T>.UnReduced(u),
                otherwise);
        }

        public static ICollection<T> Remove<T>(IEnumerable<T> coll, Predicate<T> pred) =>
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