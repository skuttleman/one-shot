using System.Collections;
using System.Collections.Generic;
using Game.System;
using Game.System.Events.Player;
using Game.Utils;
using UnityEngine;

public class PlayerAnimationStateListener : MonoBehaviour
{
    Animator animator;
    GameSession session;
    IPubSub pubsub;

    void Start()
    {
        animator = GetComponent<Animator>();
        session = FindObjectOfType<GameSession>();
        pubsub = session.Get<IPubSub>();
    }

    public void OnStanceChange(StanceChange.Stance stance)
        => pubsub.Publish(new StanceChange(stance));
    public void OnAttackMode(AttackModeChange.AttackMode mode) =>
        pubsub.Publish(new AttackModeChange(mode));
    public void OnMotionMove() =>
        pubsub.Publish(new MovementSpeedChange(animator.speed));
    public void OnMotionStop() => pubsub.Publish(new MovementSpeedChange(0f));
    public void OnScopeOn() => pubsub.Publish(new ScopeChange(true));
    public void OnScopeOff() => pubsub.Publish(new ScopeChange(false));
    public void OnStep() { }
}
