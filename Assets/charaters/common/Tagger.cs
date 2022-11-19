using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagger : MonoBehaviour
{
    [SerializeField] string[] tags;

    void OnEnable() => Register(FindObjectOfType<GameSession>());
    void Register(GameSession sess) => sess.RegisterTags(tags, gameObject);
}
