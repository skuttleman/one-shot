using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Utils;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Vector2 movement = Vector2.zero;
    AnimationListener listener;
    ISet<string> clips;

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
        listener = GetComponent<AnimationListener>();
    }

    void Update()
    {
        clips = listener.AnimationClips();
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

            if (IsAiming()) speed *= 0.9f;
            else if (IsScoping()) speed *= 0.6f;
            animator.speed = Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.y)) * speed;
            transform.position += speed * Time.deltaTime * Vectors.Upgrade(movement);
        }
    }

    public void Aim(float amount)
    {
        animator.SetBool("isAiming", amount >= 0.5);
    }

    public void Attack(bool isPressed)
    {
        if (isPressed && listener.Mode() != AttackMode.NONE)
        {
            animator.SetTrigger("attack");
        }
    }

    public void Look(Vector2 vector)
    {
        if (Vectors.NonZero(vector)) rotationZ = Vectors.AngleTo(Vector2.zero, vector);
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
        int nextStance;

        if (held && IsCrawling()) nextStance = 0;
        else if (held) nextStance = 2;
        else if (IsCrouching()) nextStance = 0;
        else nextStance = 1;

        if (nextStance != 2 || (!IsAiming() && !IsScoping()))
        {
            animator.SetInteger("stance", nextStance);
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

    bool IsStanding() => clips.Contains("stand");
    bool IsCrouching() => clips.Contains("crouch");
    bool IsCrawling() => clips.Contains("crawl");
    bool IsAiming  () => Sets.ContainsAny(clips, "aim", "toaim");
    bool IsScoping () => Sets.ContainsAny(clips, "bino", "tobino");
    bool IsMovable () => !IsCrawling() || (!IsAiming() && !IsScoping());
}
