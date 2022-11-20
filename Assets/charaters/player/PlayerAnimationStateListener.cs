using Game.System;
using Game.System.Events;
using Game.System.Events.Player;
using UnityEngine;

public class PlayerAnimationStateListener : MonoBehaviour
{
    GameSession session;
    Animator animator;
    Footstep footstep;

    IPubSub pubsub;
    PlayerStance stance;
    PlayerAttackMode mode;
    float speed;
    bool isScoping;

    void Start()
    {
        session = FindObjectOfType<GameSession>();
        animator = GetComponent<Animator>();
        footstep = GetComponentInChildren<Footstep>();
        pubsub = session.Get<IPubSub>();
    }

    void Publish<T>(bool condition, T e) where T : IEvent
    {
        if (condition) pubsub.Publish(e);
    }

    public void OnStanceChange(PlayerStance stance)
    {
        Publish(this.stance != stance, new Event<PlayerStance>(stance));
        this.stance = stance;
    }

    public void OnAttackMode(PlayerAttackMode mode)
    {
        Publish(this.mode != mode, new Event<PlayerAttackMode>(mode));
        this.mode = mode;
    }

    public void OnMovement(int moving)
    {
        float moveSpeed = moving == 0 ? 0f : animator.speed;
        Publish(speed != moveSpeed, new PlayerMovementSpeedChange(moveSpeed));
        speed = moveSpeed;
    }

    public void OnScope(int enabled)
    {
        bool isScoped = enabled != 0;
        Publish(isScoping != isScoped, new PlayerScopeChange(isScoped));
        isScoping = isScoped;
    }

    public void OnStep() {
        if (footstep)
        {
            StartCoroutine(
                Instantiate(footstep.gameObject, transform.position, transform.rotation)
                    .GetComponent<Footstep>()
                    .Go((speed + 1f) * StanceSpeed()));
        }
    }

    private float StanceSpeed()
    {
        if (stance == PlayerStance.STANDING) return 2f;
        else if (stance == PlayerStance.CRAWLING) return 0.5f;
        return 1f;
    }
}
