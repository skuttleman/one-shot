using UnityEngine;
using System.Collections;
using Game.System;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    GameSystem system;

    public T Get<T>() => system.Get<T>();

    void Awake()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        system = GameSystem.Default();
    }

    void Start()
    {
        StartCoroutine(UpdateComponents());
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
