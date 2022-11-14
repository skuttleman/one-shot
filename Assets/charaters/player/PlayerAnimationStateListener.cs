using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class PlayerAnimationStateListener : MonoBehaviour
{
    AttackMode mode = AttackMode.HAND;
    PlayerStance stance = PlayerStance.STAND;
    bool isAiming = false;
    bool isScoping = false;
    bool isMoving = false;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnStanceChange(PlayerStance stance)
    {
        this.stance = stance;
    }

    public void OnAttackMode(AttackMode mode)
    {
        this.mode = mode;
    }

    public void OnMotionChange(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    public void OnAimChange(bool isAiming)
    {
        this.isAiming = isAiming;
    }

    public void OnScopeChange(bool isScoping)
    {
        this.isScoping = isScoping;
    }

    public void OnStep()
    {
    }

    public float Speed() => animator.speed;
    public bool IsMoving() => isMoving;
    public bool IsAiming() => isAiming;
    public bool IsScoping() => isScoping;
    public AttackMode Mode() => mode;
    public PlayerStance Stance() => stance;
}

public enum AttackMode
{
    NONE, HAND, WEAPON
}

public enum PlayerStance
{
    STAND, CROUCH, CRAWL
}
