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
    public void OnMotionChange(bool isMoving) =>
        pubsub.Publish(new MovementSpeedChange(isMoving ? animator.speed : 0f));
    public void OnScopeChange(bool isScoping) =>
        pubsub.Publish(new ScopeChange(isScoping));
    public void OnStep() { }
}
