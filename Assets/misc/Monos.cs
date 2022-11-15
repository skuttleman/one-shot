using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Game.System;
using Game.System.Events.Player;
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

    public abstract class Subcriber<T> : MonoBehaviour
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

    public abstract class Subcriber<T, U> : MonoBehaviour
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

    public abstract class Subcriber<T, U, V> : MonoBehaviour
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

    public abstract class Subcriber<T, U, V, W> : MonoBehaviour
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
}