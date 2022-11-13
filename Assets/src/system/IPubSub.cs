using System;

namespace Game.System
{
    public interface IPubSub
    {
        public void Publish<T>(T e) where T : IEvent;

        public void PublishSync<T>(T e) where T : IEvent;

        public long Subscribe<T>(Action<T> action) where T : IEvent;

        public void Unsubscribe(long subscription);

        public interface IEvent { }
    }
}
