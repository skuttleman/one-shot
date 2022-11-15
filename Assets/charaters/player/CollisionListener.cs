using System;
using System.Collections;
using System.Collections.Generic;
using Game.System;
using Game.System.Events.Player;
using UnityEngine;

public class CollisionListener : Monos.Subcriber<StanceChange>
{
    [SerializeField] StanceChange.Stance stance;
    BoxCollider collide;

    void Start()
    {
        Init();
        collide = GetComponent<BoxCollider>();
    }

    public override void OnEvent(StanceChange e) =>
        collide.enabled = e.stance == stance;

    void OnDestroy() => Destroy();
}
