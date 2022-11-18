using UnityEngine;
using System;
using Game.System;
using Game.System.Events;
using Game.Utils;

public class Monos
{
    static void Unsubscribe(IPubSub pubsub, params long[] subs)
    {
        Colls.ForEach(subs, sub =>
        {
            try
            {
                pubsub.Unsubscribe(sub);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        });
    }

    public abstract class Subscriber<T> : MonoBehaviour
        where T : IEvent
    {
        IPubSub pubsub;
        long sub;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            sub = pubsub.Subscribe<T>(OnEvent);
        }

        protected void Destroy() => Unsubscribe(pubsub, sub);
        public abstract void OnEvent(T e);
    }

    public abstract class Subscriber<T, U> : MonoBehaviour
        where T : IEvent
        where U : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
    }

    public abstract class Subscriber<T, U, V> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
        public abstract void OnEvent(T e);
        public abstract void OnEvent(U e);
        public abstract void OnEvent(V e);
    }

    public abstract class Subscriber<T, U, V, W> : MonoBehaviour
        where T : IEvent
        where U : IEvent
        where V : IEvent
        where W : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where X : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where Y : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent),
                pubsub.Subscribe<Y>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where Z : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent),
                pubsub.Subscribe<Y>(OnEvent),
                pubsub.Subscribe<Z>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where A : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent),
                pubsub.Subscribe<Y>(OnEvent),
                pubsub.Subscribe<Z>(OnEvent),
                pubsub.Subscribe<A>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where B : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent),
                pubsub.Subscribe<Y>(OnEvent),
                pubsub.Subscribe<Z>(OnEvent),
                pubsub.Subscribe<A>(OnEvent),
                pubsub.Subscribe<B>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where C : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent),
                pubsub.Subscribe<Y>(OnEvent),
                pubsub.Subscribe<Z>(OnEvent),
                pubsub.Subscribe<A>(OnEvent),
                pubsub.Subscribe<B>(OnEvent),
                pubsub.Subscribe<C>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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
        where D : IEvent
    {
        IPubSub pubsub;
        long[] subs;

        protected void Init()
        {
            pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
            subs = new long[] {
                pubsub.Subscribe<T>(OnEvent),
                pubsub.Subscribe<U>(OnEvent),
                pubsub.Subscribe<V>(OnEvent),
                pubsub.Subscribe<W>(OnEvent),
                pubsub.Subscribe<X>(OnEvent),
                pubsub.Subscribe<Y>(OnEvent),
                pubsub.Subscribe<Z>(OnEvent),
                pubsub.Subscribe<A>(OnEvent),
                pubsub.Subscribe<B>(OnEvent),
                pubsub.Subscribe<C>(OnEvent),
                pubsub.Subscribe<D>(OnEvent)
            };
        }

        protected void Destroy() => Unsubscribe(pubsub, subs);
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

}