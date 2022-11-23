using System;
using System.Collections.Generic;

namespace Game.Utils {
    public delegate Reduction<A> RF<A, I>(Reduction<A> acc, I item);

    public static class Fns {
        public static Predicate<T> Compliment<T>(Predicate<T> pred) =>
            t => !pred(t);
        public static Func<T, V> Comp<T, U, V>(Func<U, V> fn1, Func<T, U> fn2) =>
            t => fn1(fn2(t));
        public static Action<T> Comp<T, U>(Action<U> action, Func<T, U> fn) =>
            t => action(fn(t));
        public static T Identity<T>(T item) => item;

        public static Func<RF<A, O>, RF<A, I>> MapCat<A, I, O>(Func<I, IEnumerable<O>> mapFn) =>
            rf => (acc, item) => mapFn(item).Reduce((a, i) => rf(a, i), acc);
        public static Func<RF<A, O>, RF<A, I>> Map<A, I, O>(Func<I, O> fn) =>
            rf => (acc, item) => rf(acc, fn(item));
        public static Func<RF<A, I>, RF<A, I>> Filter<A, I>(Predicate<I> pred) =>
            rf => (acc, item) => pred(item) ? acc : rf(acc, item);
        public static Func<RF<A, I>, RF<A, I>> Remove<A, I>(Predicate<I> pred) =>
            Filter<A, I>(Compliment(pred));
        public static Func<RF<A, I>, RF<A, I>> Take<A, I>(long n) => rf => {
            long items = n;
            return (acc, item) =>
                items-- > 0 ? rf(acc, item) : Reduction<A>.Reduced(acc.Get());
        };
    }

    public class MultiMethod<T, U, R> {
        readonly IDictionary<U, Func<T, R>> dict;
        readonly Func<T, U> dispatchFn;
        readonly R defaultVal;

        public static MultiMethod<T, U, R> Over(Func<T, U> dispatchFn) {
            return Over(dispatchFn, default);
        }

        public static MultiMethod<T, U, R> Over(Func<T, U> dispatchFn, R defaultVal) {
            IDictionary<U, Func<T, R>> dict = new Dictionary<U, Func<T, R>>();
            return new MultiMethod<T, U, R>(dict, dispatchFn, defaultVal);
        }

        private MultiMethod(IDictionary<U, Func<T, R>> dict, Func<T, U> dispatchFn, R defaultVal) {
            this.dict = dict;
            this.dispatchFn = dispatchFn;
            this.defaultVal = defaultVal;
        }

        public MultiMethod<T, U, R> AddMethod(U dispatchVal, Func<T, R> fn) {
            dict[dispatchVal] = fn;
            return this;
        }

        public MultiMethod<T, U, R> AddMethod(U dispatchVal, Action<T> action) {
            dict[dispatchVal] = input => { action(input); return default; };
            return this;
        }

        public MultiMethod<T, U, R> RemoveMethod(U dispatchVal) {
            dict.Remove(dispatchVal);
            return this;
        }

        public Func<T, R> Func() {
            return input => {
                U dispatchVal = dispatchFn(input);
                return dict.ContainsKey(dispatchVal)
                    ? dict[dispatchVal](input)
                    : defaultVal;
            };
        }

        public Action<T> Action() {
            return input => Func()(input);
        }
    }
}
