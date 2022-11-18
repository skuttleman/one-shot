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
    public void OnMotionMove()
    {
        Publish(speed != animator.speed, new MovementSpeedChange(animator.speed));
        speed = animator.speed;
    }

    public void OnMotionStop()
    {
        Publish(speed != 0f, new MovementSpeedChange(0f));
        speed = 0f;
    }

    public void OnScopeOn()
    {
        Publish(!isScoping, new ScopeChange(true));
        isScoping = true;
    }

    public void OnScopeOff()
    {
        Publish(isScoping, new ScopeChange(false));
        isScoping = false;
    }

    public void OnStep() { }
}
