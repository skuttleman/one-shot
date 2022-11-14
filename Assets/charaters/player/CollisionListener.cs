using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionListener : MonoBehaviour
{
    [SerializeField] PlayerStance stance;
    PlayerAnimationStateListener listener;
    BoxCollider collide;

    void Start()
    {
        listener = FindObjectOfType<PlayerAnimationStateListener>();
        collide = GetComponent<BoxCollider>();
    }


    void Update()
    {
        collide.enabled = listener.Stance() == stance;
    }
}
