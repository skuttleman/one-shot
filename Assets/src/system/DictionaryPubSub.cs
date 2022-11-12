using UnityEngine;
using System.Collections;
using Game.System;
using System;
using System.Collections.Generic;

namespace Game.System
{
    public class DictionaryPubSub : IComponent, IPubSub
    {
        IDictionary<Type, IDictionary<int, Action<IPubSub.IEvent>>> actions;
        IDictionary<int, Type> subscribers;
        Queue<IPubSub.IEvent> q;

        int subId = 0;

        public DictionaryPubSub()
        {
            actions = new Dictionary<Type, IDictionary<int, Action<IPubSub.IEvent>>>();
            subscribers = new Dictionary<int, Type>();
            q = new Queue<IPubSub.IEvent>();
        }

        public void Publish<T>(T e) where T : IPubSub.IEvent
        {
            q.Enqueue(e);
        }

        public int Subscribe<T>(Action<IPubSub.IEvent> action) where T : IPubSub.IEvent
        {
            int id = ++subId;
            Type t = typeof(T);
            subscribers.Add(id, t);
            IDictionary<int, Action<IPubSub.IEvent>> dict = actions.ContainsKey(t) ? actions[t] : new Dictionary<int, Action<IPubSub.IEvent>>();
            dict.Add(id, action);
            actions.Add(t, dict);

            return id;
        }

        public void Unsubscribe(int subscription)
        {
            Type t = subscribers.ContainsKey(subscription) ? subscribers[subscription] : null;
            if (t != null)
            {
                IDictionary<int, Action<IPubSub.IEvent>> dict = actions[t];
                dict.Remove(subscription);
                subscribers.Remove(subscription);
            }
        }

        public void Tick()
        {
            if (q.Count > 0)
            {
                IPubSub.IEvent e = q.Dequeue();
                foreach (KeyValuePair<int, Action<IPubSub.IEvent>> entry in actions[e.GetType()])
                {
                    entry.Value(e);
                }
            }
        }
    }
}