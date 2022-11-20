using Game.System;
using System;
using System.Collections.Generic;
using Game.Utils;
using System.Collections;

public class GameSystem : IEnumerable<(Type, IComponent)>
{
    private readonly IDictionary<Type, IComponent> components;

    private GameSystem(IDictionary<Type, IComponent> components)
    {
        this.components = components;
    }

    public T Get<T>()
    {
        Type key = typeof(T);
        return components.ContainsKey(key)
            ? (T)components[key]
            : default;
    }

    public void Register<T>(T component) where T : IComponent
    {
        components.Add(typeof(T), component);
    }

    public void Unregister(Type type)
    {
        components.Remove(type);
    }


    public static GameSystem Default(GameSession session)
    {
        DictionaryPubSub pubsub = new();

        return new GameSystem(
            new Dictionary<Type, IComponent>
            {
                { typeof(IPubSub), pubsub },
            });
    }

    public IEnumerator<(Type, IComponent)> GetEnumerator() =>
        Sequences
            .Map(components, entry => (entry.Key, entry.Value))
            .GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
