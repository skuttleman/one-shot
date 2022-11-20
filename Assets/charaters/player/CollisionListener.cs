using Game.System.Events.Player;
using Game.Utils.Mono;
using UnityEngine;

public class CollisionListener : Subscriber<StanceChange>
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
