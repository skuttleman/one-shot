﻿using UnityEngine;
using System.Collections;
using Game.System;
using System;
using System.Collections.Generic;

public class GameSystem
{
    IDictionary<Type, IComponent> components;

    private GameSystem(IDictionary<Type, IComponent> components)
    {
        this.components = components;
    }

    public T Get<T>()
    {
        return (T)components[typeof(T)];
    }

    public static GameSystem Default()
    {
        return new GameSystem(
            new Dictionary<Type, IComponent>
            {
                { typeof(IPubSub), new DictionaryPubSub() }
            });
    }

    public ICollection<IComponent> Components()
    {
        return components.Values;
    }
}