using System;
using System.Collections.Generic;

namespace Game.Utils {
    public static class Colls {
        public static L Add<L, T>(L coll, T item) where L : ICollection<T> {
            coll.Add(item);
            return coll;
        }

        public static Func<T, U> Fn<T, U>(IDictionary<T, U> dict) =>
            item => dict.ContainsKey(item) ? dict[item] : default;
        public static Predicate<T> Pred<T, U>(IDictionary<T, U> dict) =>
            item => dict.ContainsKey(item);
        public static Predicate<T> Pred<T>(ISet<T> set) =>
            item => set.Contains(item);

        public static U Get<T, U>(IDictionary<T, U> dict, T key) {
            if (dict.ContainsKey(key)) return dict[key];
            return default;
        }

        public static Sequence<long> Range() => Sequences.Iterate(0L, x => x + 1);
        public static Sequence<long> Range(long end) => Range(0L, end);
        public static Sequence<long> Range(long start, long end) => Range(start, end, 1L);
        public static Sequence<long> Range(long start, long end, long jump) =>
            Sequence<long>.Of(RangeEnumerable(start, end, jump));

        private static IEnumerable<long> RangeEnumerable(long start, long end, long jump) {
            for (long i = start; i < end; i += jump)
                yield return i;
        }
    }
}
