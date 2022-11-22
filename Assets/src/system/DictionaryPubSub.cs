using System;
using System.Collections.Generic;
using Game.Utils;
using Game.System.Events;
using System.Collections.Concurrent;

namespace Game.System {
    public class DictionaryPubSub : IComponent, IPubSub<IEvent> {
        IDictionary<Type, IDictionary<long, Action<IEvent>>> actions;
        IDictionary<long, Type> subscribers;
        readonly ConcurrentQueue<IEvent> q;

        readonly object idLock = new();
        long subId = 0;

        public void Tick(GameSession _) {
            while (q.TryDequeue(out IEvent e)) PublishEvent(e);
        }

        public DictionaryPubSub() {
            actions = new Dictionary<Type, IDictionary<long, Action<IEvent>>>();
            subscribers = new Dictionary<long, Type>();
            q = new ConcurrentQueue<IEvent>();
        }

        public IPubSub<IEvent> Publish<T>(T e) where T : IEvent {
            q.Enqueue(e);
            return this;
        }

        public IPubSub<IEvent> PublishSync<T>(T e) where T : IEvent {
            PublishEvent(e);
            return this;
        }

        public long Subscribe<T>(Action<T> action) where T : IEvent {
            lock (idLock) {
                ++subId;
            }
            Type t = typeof(T);
            IDictionary<long, Action<IEvent>> dict = actions.ContainsKey(t)
                ? actions[t]
                : new Dictionary<long, Action<IEvent>>();
            subscribers[subId] = t;
            dict[subId] = e => action((T)e);
            actions[t] = dict;

            return subId;
        }

        public IPubSub<IEvent> Unsubscribe(long subscription) {
            Type t = subscribers.ContainsKey(subscription) ? subscribers[subscription] : null;
            if (t != null) {
                IDictionary<long, Action<IEvent>> dict = actions[t];
                dict.Remove(subscription);
                subscribers.Remove(subscription);
            }
            return this;
        }

        void PublishEvent(IEvent e) {
            Sequences.ForEach(Colls.Get(actions, e.GetType()), entry => entry.Value(e));
        }
    }
}