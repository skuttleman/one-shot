using UnityEngine;
using System.Collections;
using Game.System;
using System;
using System.Collections.Generic;
using Game.Utils;

namespace Game.System
{
    public class DictionaryPubSub : IComponent, IPubSub
    {
        IDictionary<Type, IDictionary<long, Action<IPubSub.IEvent>>> actions;
        IDictionary<long, Type> subscribers;
        Queue<IPubSub.IEvent> q;

        long subId = 0;

        public void Tick()
        {
            while (q.Count > 0) PublishEvent(q.Dequeue());
        }

        public DictionaryPubSub()
        {
            actions = new Dictionary<Type, IDictionary<long, Action<IPubSub.IEvent>>>();
            subscribers = new Dictionary<long, Type>();
            q = new Queue<IPubSub.IEvent>();
        }

        public void Publish<T>(T e) where T : IPubSub.IEvent
        {
            q.Enqueue(e);
        }

        public void PublishSync<T>(T e) where T : IPubSub.IEvent
        {
            PublishEvent(e);
        }

        public long Subscribe<T>(Action<T> action) where T : IPubSub.IEvent
        {
            long id = ++subId;
            Type t = typeof(T);
            IDictionary<long, Action<IPubSub.IEvent>> dict = actions.ContainsKey(t)
                ? actions[t]
                : new Dictionary<long, Action<IPubSub.IEvent>>();
            subscribers.Add(id, t);
            dict.Add(id, (IPubSub.IEvent e) => action((T)e));
            actions.Add(t, dict);

            return id;
        }

        public void Unsubscribe(long subscription)
        {
            Type t = subscribers.ContainsKey(subscription) ? subscribers[subscription] : null;
            if (t != null)
            {
                IDictionary<long, Action<IPubSub.IEvent>> dict = actions[t];
                dict.Remove(subscription);
                subscribers.Remove(subscription);
            }
        }

        void PublishEvent(IPubSub.IEvent e)
        {
            Colls.ForEach(actions[e.GetType()], entry => entry.Value(e));
        }
    }
}