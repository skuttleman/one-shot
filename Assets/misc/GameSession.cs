using UnityEngine;
using System.Collections;
using Game.System;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Game.Utils;

public class GameSession : MonoBehaviour
{
    IDictionary<string, ISet<GameObject>> taggedObjects;
    GameSystem system;

    public void RegisterTags(IEnumerable<string> tags, GameObject obj)
    {
        Colls.ForEach(tags, tag =>
        {
            ISet<GameObject> objects = new HashSet<GameObject>();
            if (taggedObjects.ContainsKey(tag)) objects = taggedObjects[tag];
            else taggedObjects[tag] = objects;
            if (!objects.Contains(obj)) objects.Add(obj);
        });
    }

    public GameObject GetTaggedObject(string tag)
    {
        ISet<GameObject> objects = GetTaggedObjects(tag);
        if (objects.Count > 1)
            throw new InvalidOperationException("More than one item found for tag: " + tag);
        return Colls.First(objects);
    }

    public T Get<T>() => system.Get<T>();
    public ISet<GameObject> GetTaggedObjects(string tag) => Colls.Get(taggedObjects, tag);
    public GameObject GetPlayer() => GetTaggedObject("player");

    void Awake()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        system = GameSystem.Default();
        taggedObjects = new Dictionary<string, ISet<GameObject>>();
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
