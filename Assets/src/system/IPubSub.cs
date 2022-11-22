using System;

namespace Game.System {
    public interface IPubSub<E> {
        public IPubSub<E> Publish<T>(T e) where T : E;
        public IPubSub<E> PublishSync<T>(T e) where T : E;
        public long Subscribe<T>(Action<T> action) where T : E;
        public IPubSub<E> Unsubscribe(long subscription);
    }
}
