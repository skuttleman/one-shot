using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Utils;
using UnityEngine.InputSystem;
using System;
using Game.System.Events.Player;

public class PlayerController : Monos.Subcriber<
        StanceChange, AttackModeChange,
        MovementSpeedChange, ScopeChange
    >
{
    Animator animator;
    Vector2 movement = Vector2.zero;
    PlayerAnimationStateListener listener;

    // animation state
    AttackModeChange.AttackMode mode;
    StanceChange.Stance stance;
    bool isMoving;
    bool isAiming;
    bool isScoping;

    // movement modifiers
    [SerializeField] float walkSpeed = 2.5f;
    [SerializeField] float crouchSpeed = 1.5f;
    [SerializeField] float crawlSpeed = 0.75f;
    [SerializeField] float rotationSpeed = 8f;
    float rotationZ = 0f;
    float movementModifer = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        listener = GetComponent<PlayerAnimationStateListener>();
    }

    void Update()
    {
        RotatePlayer();
        MovePlayer();
    }

    void RotatePlayer()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);
    }

    void MovePlayer()
    {
        if (IsMovable())
        {
            float speed = movementModifer * StanceSpeed();

            if (isAiming) speed *= 0.9f;
            else if (isScoping) speed *= 0.6f;
            float movementSpeed = Mathf.Max(
                Mathf.Abs(movement.x),
                Mathf.Abs(movement.y));
            animator.speed = movementSpeed * speed;
            transform.position += speed * Time.deltaTime * Vectors.Upgrade(movement);
        }
    }

    public void Aim(float amount)
    {
        animator.SetBool("isAiming", amount >= 0.5);
    }

    public void Attack(bool isPressed)
    {
        if (isPressed && mode != AttackModeChange.AttackMode.NONE)
        {
            animator.SetTrigger("attack");
        }
    }

    public void Look(Vector2 vector)
    {
        if (Vectors.NonZero(vector))
            rotationZ = Vectors.AngleTo(Vector2.zero, vector);
    }

    public void Move(Vector2 vector)
    {
        movement = vector;
        bool isMoving = Vectors.NonZero(movement);
        if (isMoving) rotationZ = Vectors.AngleTo(Vector2.zero, movement);
        animator.SetBool("isMoving", isMoving);
    }

    public void Binoculars(bool isPressed)
    {
        animator.SetBool("isScoping", isPressed);
    }

    public void Stance(float amount)
    {
        bool held = amount >= 0.35f;
        StanceChange.Stance nextStance;

        if (held && IsCrawling()) nextStance = StanceChange.Stance.STANDING;
        else if (held) nextStance = StanceChange.Stance.CRAWLING;
        else if (IsCrouching()) nextStance = StanceChange.Stance.STANDING;
        else nextStance = StanceChange.Stance.CROUCHING;

        if (nextStance != StanceChange.Stance.CRAWLING
            || (!isAiming && !isScoping)
            || !isMoving)
        {
            stance = nextStance;
            animator.SetInteger("stance", (int)stance);
        }
    }

    public void MoveModifier(bool isPressed)
    {
        if (isPressed) movementModifer = 0.5f;
        else movementModifer = 1f;
    }

    float StanceSpeed()
    {
        if (IsCrouching()) return crouchSpeed;
        if (IsCrawling()) return crawlSpeed;
        return walkSpeed;
    }

    bool IsCrawling() => stance == StanceChange.Stance.CRAWLING;
    bool IsCrouching() => stance == StanceChange.Stance.CROUCHING;
    bool IsMovable() => !IsCrawling() || (!isAiming && !isScoping);
    public override void OnEvent(ScopeChange e) => isScoping = e.isScoping;
    public override void OnEvent(StanceChange e) => stance = e.stance;
    public override void OnEvent(AttackModeChange e) => mode = e.mode;
    public override void OnEvent(MovementSpeedChange e) => isMoving = Maths.NonZero(e.speed);
}
