using UnityEngine;
using Game.System;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Game.Utils;
using System.Threading;

public class GameSession : MonoBehaviour {
    IDictionary<string, ISet<GameObject>> taggedObjects;
    GameSystem system;
    IDictionary<Type, Thread> threads;

    public void RegisterTags(IEnumerable<string> tags, GameObject obj) {
        Sequences.ForEach(tags, tag => {
            ISet<GameObject> objects = new HashSet<GameObject>();
            if (taggedObjects.ContainsKey(tag)) objects = taggedObjects[tag];
            else taggedObjects[tag] = objects;
            if (!objects.Contains(obj)) objects.Add(obj);
        });
    }

    public GameObject GetTaggedObject(string tag) {
        ISet<GameObject> objects = GetTaggedObjects(tag);
        if (objects.Count > 1)
            throw new InvalidOperationException("More than one item found for tag: " + tag);
        return Sequences.First(objects);
    }

    public ISet<GameObject> GetTaggedObjects(string tag) => Colls.Get(taggedObjects, tag);
    public GameObject GetPlayer() => GetTaggedObject("player");
    public T Get<T>() => system.Get<T>();
    public void Register<T>(T component) where T : IComponent {
        StartComponent(typeof(T), component);
        system.Register(component);
    }

    public void Unregister<T>() {
        system.Unregister(typeof(T));
        StopComponent(typeof(T));
    }

    void Awake() {
        if (FindObjectsOfType<GameSession>().Length > 1) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        system = GameSystem.Default(this);
        taggedObjects = new Dictionary<string, ISet<GameObject>>();
        threads = new Dictionary<Type, Thread>();
        Sequences.ForEach(system, tpl => StartComponent(tpl.Item1, tpl.Item2));
    }

    void Start() {
        // TODO - nocommit
        InputSystem.DisableDevice(Mouse.current);
    }

    void StartComponent(Type type, IComponent component) {
        if (threads.ContainsKey(type))
            throw new NotSupportedException("component has already been started");

        threads[type] = new(new ThreadStart(() => {
            while (true) {
                component.Tick(this);
                Thread.Sleep(1);
            }
        }));
        threads[type].Start();
    }

    void StopComponent(Type type) {
        Thread thread = Colls.Get(threads, type);
        if (thread != null) thread.Abort();
    }

    void OnDestroy() => Sequences.ForEach(system, tpl => StopComponent(tpl.Item1));
}
