using System;
using Game.System.Events;

namespace Game.System
{
    public interface IPubSub
    {
        public IPubSub Publish<T>(T e) where T : IEvent;

        public long Subscribe<T>(Action<T> action) where T : IEvent;

        public IPubSub Unsubscribe(long subscription);
    }
}
