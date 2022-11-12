using System;

namespace Game.System
{
    public interface IPubSub
    {
        public void Publish<T>(T e) where T : IEvent;

        public int Subscribe<T>(Action<T> action) where T : IEvent;

        public void Unsubscribe(int subscription);

        public interface IEvent { }
    }
}
