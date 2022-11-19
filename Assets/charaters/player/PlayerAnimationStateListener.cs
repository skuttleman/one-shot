using System.Collections;
using System.Collections.Generic;
using Game.System;
using Game.System.Events;
using Game.System.Events.Player;
using Game.Utils;
using UnityEngine;

public class PlayerAnimationStateListener : MonoBehaviour
{
    Animator animator;
    GameSession session;
    IPubSub pubsub;
    StanceChange.Stance stance;
    AttackModeChange.AttackMode mode;
    float speed;
    bool isScoping;

    void Start()
    {
        animator = GetComponent<Animator>();
        session = FindObjectOfType<GameSession>();
        pubsub = session.Get<IPubSub>();
    }

    void Publish<T>(bool condition, T e) where T : IEvent
    {
        if (condition) pubsub.Publish(e);
    }

    public void OnStanceChange(StanceChange.Stance stance)
    {
        Publish(this.stance != stance, new StanceChange(stance));
        this.stance = stance;
    }

    public void OnAttackMode(AttackModeChange.AttackMode mode)
    {
        Publish(this.mode != mode, new AttackModeChange(mode));
        this.mode = mode;
    }

    public void OnMove(int moving)
    {
        float moveSpeed = moving == 0 ? 0f : animator.speed;
        Publish(speed != moveSpeed, new MovementSpeedChange(moveSpeed));
        speed = moveSpeed;
    }

    public void OnScope(int enabled)
    {
        bool isScoped = enabled != 0;
        Publish(isScoping != isScoped, new ScopeChange(isScoped));
        isScoping = isScoped;
    }

    public void OnStep() { }
}
