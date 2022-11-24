using System;
using UnityEngine;

public class Tagger : MonoBehaviour
{
    GameSession session;
    [SerializeField] string[] tags;

    void Start() {
        session = FindObjectOfType<GameSession>();
        session.RegisterTags(tags, gameObject);
    }

    void OnDestroy() => session?.UnregisterTags(gameObject);
}
