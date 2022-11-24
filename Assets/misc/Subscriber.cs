using UnityEngine;
using System;
using Game.System;
using Game.System.Events;
using System.Collections.Concurrent;

namespace Game.Utils.Mono {
    public abstract class Subscriber<T> : MonoBehaviour
        where T : IEvent {
        readonly ConcurrentQueue<IEvent> q = new();
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
    }

    public abstract class Subscriber<T, U> : MonoBehaviour
        where T : IEvent
        where U : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
    }

    public abstract class Subscriber<T, U, V> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
    }

    public abstract class Subscriber<T, U, V, W> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
    }

    public abstract class Subscriber<T, U, V, W, X> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
    }

    public abstract class Subscriber<T, U, V, W, X, Y> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent
        where Y : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e)),
                pubsub.Subscribe<Y>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
                else if (e.GetType() == typeof(Y)) OnEvent((Y)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
        public abstract void OnEvent(Y e);
    }

    public abstract class Subscriber<T, U, V, W, X, Y, Z> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent
        where Y : IEvent
        where Z : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e)),
                pubsub.Subscribe<Y>(e => q.Enqueue(e)),
                pubsub.Subscribe<Z>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
                else if (e.GetType() == typeof(Y)) OnEvent((Y)e);
                else if (e.GetType() == typeof(Z)) OnEvent((Z)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
        public abstract void OnEvent(Y e);
        public abstract void OnEvent(Z e);
    }

    public abstract class Subscriber<T, U, V, W, X, Y, Z, A> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent
        where Y : IEvent
        where Z : IEvent
        where A : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e)),
                pubsub.Subscribe<Y>(e => q.Enqueue(e)),
                pubsub.Subscribe<Z>(e => q.Enqueue(e)),
                pubsub.Subscribe<A>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
                else if (e.GetType() == typeof(Y)) OnEvent((Y)e);
                else if (e.GetType() == typeof(Z)) OnEvent((Z)e);
                else if (e.GetType() == typeof(A)) OnEvent((A)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
        public abstract void OnEvent(Y e);
        public abstract void OnEvent(Z e);
        public abstract void OnEvent(A e);
    }

    public abstract class Subscriber<T, U, V, W, X, Y, Z, A, B> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent
        where Y : IEvent
        where Z : IEvent
        where A : IEvent
        where B : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e)),
                pubsub.Subscribe<Y>(e => q.Enqueue(e)),
                pubsub.Subscribe<Z>(e => q.Enqueue(e)),
                pubsub.Subscribe<A>(e => q.Enqueue(e)),
                pubsub.Subscribe<B>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
                else if (e.GetType() == typeof(Y)) OnEvent((Y)e);
                else if (e.GetType() == typeof(Z)) OnEvent((Z)e);
                else if (e.GetType() == typeof(A)) OnEvent((A)e);
                else if (e.GetType() == typeof(B)) OnEvent((B)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
        public abstract void OnEvent(Y e);
        public abstract void OnEvent(Z e);
        public abstract void OnEvent(A e);
        public abstract void OnEvent(B e);
    }

    public abstract class Subscriber<T, U, V, W, X, Y, Z, A, B, C> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent
        where Y : IEvent
        where Z : IEvent
        where A : IEvent
        where B : IEvent
        where C : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e)),
                pubsub.Subscribe<Y>(e => q.Enqueue(e)),
                pubsub.Subscribe<Z>(e => q.Enqueue(e)),
                pubsub.Subscribe<A>(e => q.Enqueue(e)),
                pubsub.Subscribe<B>(e => q.Enqueue(e)),
                pubsub.Subscribe<C>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
                else if (e.GetType() == typeof(Y)) OnEvent((Y)e);
                else if (e.GetType() == typeof(Z)) OnEvent((Z)e);
                else if (e.GetType() == typeof(A)) OnEvent((A)e);
                else if (e.GetType() == typeof(B)) OnEvent((B)e);
                else if (e.GetType() == typeof(C)) OnEvent((C)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
        public abstract void OnEvent(Y e);
        public abstract void OnEvent(Z e);
        public abstract void OnEvent(A e);
        public abstract void OnEvent(B e);
        public abstract void OnEvent(C e);
    }

    public abstract class Subscriber<T, U, V, W, X, Y, Z, A, B, C, D> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
        where X : IEvent
        where Y : IEvent
        where Z : IEvent
        where A : IEvent
        where B : IEvent
        where C : IEvent
        where D : IEvent {
        ConcurrentQueue<IEvent> q;
        internal IPubSub<IEvent> pubsub;
        long[] subs;

        protected void Start() {
            q = new();
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub<IEvent>>();
            subs = new long[] {
                pubsub.Subscribe<T>(e => q.Enqueue(e)),
                pubsub.Subscribe<U>(e => q.Enqueue(e)),
                pubsub.Subscribe<V>(e => q.Enqueue(e)),
                pubsub.Subscribe<W>(e => q.Enqueue(e)),
                pubsub.Subscribe<X>(e => q.Enqueue(e)),
                pubsub.Subscribe<Y>(e => q.Enqueue(e)),
                pubsub.Subscribe<Z>(e => q.Enqueue(e)),
                pubsub.Subscribe<A>(e => q.Enqueue(e)),
                pubsub.Subscribe<B>(e => q.Enqueue(e)),
                pubsub.Subscribe<C>(e => q.Enqueue(e)),
                pubsub.Subscribe<D>(e => q.Enqueue(e))
            };
        }

        protected void Update() {
            while (q.TryDequeue(out IEvent e))
                if (e.GetType() == typeof(T)) OnEvent((T)e);
                else if (e.GetType() == typeof(U)) OnEvent((U)e);
                else if (e.GetType() == typeof(V)) OnEvent((V)e);
                else if (e.GetType() == typeof(W)) OnEvent((W)e);
                else if (e.GetType() == typeof(X)) OnEvent((X)e);
                else if (e.GetType() == typeof(Y)) OnEvent((Y)e);
                else if (e.GetType() == typeof(Z)) OnEvent((Z)e);
                else if (e.GetType() == typeof(A)) OnEvent((A)e);
                else if (e.GetType() == typeof(B)) OnEvent((B)e);
                else if (e.GetType() == typeof(C)) OnEvent((C)e);
                else if (e.GetType() == typeof(D)) OnEvent((D)e);
        }

        protected void OnDestroy() => SubscriberUtils.Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
        public abstract void OnEvent(W e);
        public abstract void OnEvent(X e);
        public abstract void OnEvent(Y e);
        public abstract void OnEvent(Z e);
        public abstract void OnEvent(A e);
        public abstract void OnEvent(B e);
        public abstract void OnEvent(C e);
        public abstract void OnEvent(D e);
    }

    static class SubscriberUtils {
        public static void Unsubscribe(IPubSub<IEvent> pubsub, params long[] subs) {
            subs.ForEach(sub => {
                try {
                    pubsub.Unsubscribe(sub);
                } catch (Exception ex) {
                    Debug.LogException(ex);
                }
            });
        }
    }
}
