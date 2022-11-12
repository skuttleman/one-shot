using UnityEngine;
using System.Collections;
using Game.System;
using System;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    GameSystem system;

    void Awake()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        system = GameSystem.Default();
        StartCoroutine(UpdateComponents());
    }

    public T Get<T>()
    {
        return system.Get<T>();
    }

    IEnumerator UpdateComponents()
    {
        while (true)
        {
            foreach (IComponent component in system.Components()) component.Tick();
            yield return new WaitForEndOfFrame();
        }
    }
}
