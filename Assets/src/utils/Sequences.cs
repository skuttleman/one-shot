using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils {
    public static class Sequences {
        // void reduction
        public static void DoAll<T>(this IEnumerable<T> coll) => ForEach(coll, _ => { });
        public static void ForEach<T>(this IEnumerable<T> coll, Action<T> action) {
            if (coll != null) foreach (T item in coll) action(item);
        }

        // value reduction
        public static U Reduce<T, U>(this IEnumerable<T> coll, Func<U, T, U> reducer, U init) {
            U result = init;
            if (coll != null) {
                foreach (T item in coll) result = reducer(result, item);
            }
            return result;
        }
        public static U ReduceUntil<T, U>(this IEnumerable<T> coll, Func<U, T, Reduction<U>> reducer, U init) {
            U result = init;
            if (coll != null) {
                foreach (T item in coll) {
                    Reduction<U> next = reducer(result, item);
                    result = next.Get();
                    if (next.IsReduced()) return result;
                }
            }
            return result;
        }

        // sequence transposition
        public static IEnumerable<T> Rest<T>(this IEnumerable<T> coll) {
            IEnumerator<T> en = coll.GetEnumerator();
            en.MoveNext();
            while (en.MoveNext()) yield return en.Current;
        }
        public static IEnumerable<U> MapCat<T, U>(this IEnumerable<T> coll, Func<T, IEnumerable<U>> fn) =>
            _Expand(coll, fn);
        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> coll, Func<T, U> fn) =>
            _Expand(coll, item => new U[] { fn(item) });
        public static IEnumerable<V> Map<T, U, V>(
            this IEnumerable<T> coll1,
            IEnumerable<U> coll2,
            Func<T, U, V> fn) => _Expand(
                coll1,
                coll2,
                (item1, item2) => new V[] { fn(item1, item2) });
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> coll, Predicate<T> pred) =>
            _Expand(coll, item => pred(item) ? Of(item) : Empty<T>());
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> coll, Predicate<T> pred) =>
            Filter(coll, Fns.Compliment(pred));
        public static IEnumerable<T> Take<T>(this IEnumerable<T> coll, long n) {
            long items = n;
            foreach (T item in coll) {
                if (items-- > 0) yield return item;
                else yield break;
            }
        }
        public static IEnumerable<T> Drop<T>(this IEnumerable<T> coll, long n) =>
            n > 0 ? Drop(Rest(coll), n - 1) : coll;

        // generation
        public static IEnumerable<T> Empty<T>() {
            yield break;
        }
        public static IEnumerable<T> Of<T>(params T[] items) {
            foreach (T item in items) yield return item;
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> coll, params IEnumerable<T>[] seqs) {
            foreach (IEnumerable<T> seq in Cons(coll, seqs))
                foreach (T item in seq)
                    yield return item;
        }
        public static IEnumerable<T> Iterate<T>(T init, Func<T, T> nextFn) {
            T value = init;
            while (true) {
                yield return value;
                value = nextFn(value);
            }
        }
        public static IEnumerable<T> Cons<T>(T head, IEnumerable<T> tail) {
            yield return head;
            foreach (T item in tail) yield return item;
        }
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> coll) {
            while (true)
                foreach (T item in coll)
                    yield return item;
        }

        // extraction
        public static T Find<T>(this IEnumerable<T> coll, Predicate<T> pred) =>
        coll.Filter(pred).First();
        public static T First<T>(this IEnumerable<T> coll) {
            if (coll != null) foreach (T item in coll) return item;
            return default;
        }

        // transduction
        public static A Transduce<A, I, O>(
            this IEnumerable<I> coll,
            IXForm<I, O> xform,
            Func<A, O, A> reducer,
            A init) {
            RF<A, I> reduceFn = xform.XForm<A>((red, item) => red.IsReduced() ?
                red :
                Reduction<A>.UnReduced(reducer(red.Get(), item)));
            return coll.ReduceUntil(
                (acc, item) => reduceFn(Reduction<A>.UnReduced(acc), item),
                init);
        }
        public static IEnumerable<O> Sequence<I, O>(this IEnumerable<I> coll, IXForm<I, O> xform) {
            return coll.Map(_SeqStepFn(xform))
                .Map(x => { Debug.Log("PEEKING " + x.GetType()); return x; })
                .Filter(item => item.IsReduced())
                .Map(item => item.Get());
        }

        // internal
        private static Func<I, Reduction<O>> _SeqStepFn<I, O>(IXForm<I, O> xform) {
            RF<O, I> rf = xform.XForm<O>((_, item) => Reduction<O>.Reduced(item));
            return item => rf(default, item);
        }
        private static IEnumerable<U> _Expand<T, U>(IEnumerable<T> coll, Func<T, IEnumerable<U>> expander) {
            if (coll != null)
                foreach (T input in coll)
                    foreach (U item in expander(input))
                        yield return item;
        }
        private static IEnumerable<V> _Expand<T, U, V>(
            IEnumerable<T> coll1,
            IEnumerable<U> coll2,
            Func<T, U, IEnumerable<V>> expander) {
            if (coll1 != null && coll2 != null) {
                IEnumerator<T> iter1 = coll1.GetEnumerator();
                IEnumerator<U> iter2 = coll2.GetEnumerator();

            while (true) {
                if ((iter1?.MoveNext() ?? false) && (iter2?.MoveNext() ?? false))
                    foreach (V item in expander(iter1.Current, iter2.Current))
                        yield return item;
                else yield break;
            }
        }
    }

    public class Reduction<T> {
        readonly T item;
        readonly bool isReduced;

        private Reduction(T item, bool isReduced) {
            this.item = item;
            this.isReduced = isReduced;
        }

        public static Reduction<T> Reduced(T item) => new(item, true);
        public static Reduction<T> UnReduced(T item) => new(item, false);
        public bool IsReduced() => isReduced;
        public T Get() => item;
    }
}
