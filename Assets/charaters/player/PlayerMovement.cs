using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Utils;
using System;
using Game.System;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    float rotationSpeed = 8f;
    float rotationZ = 0f;
    Vector2 movement = Vector2.zero;
    GameSession session;

    // animation state
    bool isMoving = false;
    bool isScoping = false;
    bool isAiming = false;
    bool isLooking = false;
    int stance = 0;
    AttackMode mode = AttackMode.HAND;

    void Start()
    {
        animator = GetComponent<Animator>();
        session = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);
        transform.position += Vectors.Upgrade(movement) * Time.deltaTime;
    }

    void OnAim(InputValue value)
    {
        if (stance < 2 || !isMoving)
        {
            isAiming = value.Get<float>() >= 0.5;
            animator.SetBool("isAiming", isAiming);
        }
    }

    void OnAttack(InputValue value)
    {
    }

    void OnLook(InputValue value)
    {
        Vector2 rotation = value.Get<Vector2>();
        isLooking = rotation != Vector2.zero;
        if (isLooking) rotationZ = Vectors.Angle(Vector2.zero, rotation);
    }

    void OnMove(InputValue value)
    {
        Vector2 moveAmount = value.Get<Vector2>();

        if (stance < 2 || (!isAiming && !isScoping))
        {
            movement = moveAmount;
            isMoving = movement != Vector2.zero;
            animator.SetBool("isMoving", isMoving);
            if (isMoving) rotationZ = Vectors.Angle(Vector2.zero, movement);
        }
        else if (moveAmount != Vector2.zero)
        {
            rotationZ = Vectors.Angle(Vector2.zero, moveAmount);

        }
    }

    void OnBinoculars(InputValue value)
    {
        if (stance < 2 || !isMoving)
        {
            isScoping = value.isPressed;
            animator.SetBool("isScoping", isScoping);
        }
    }

    void OnStance(InputValue value)
    {
        bool held = value.Get<float>() >= 0.5f;
        stance = animator.GetInteger("stance");

        if (held && stance == 2)
        {
            stance = 0;
        }
        else if (held)
        {
            stance = 2;
            if (isMoving)
            {
                isAiming = false;
                isScoping = false;
                animator.SetBool("isAiming", isAiming);
                animator.SetBool("isScoping", isScoping);
            }
        }
        else if (stance == 1)
        {
            stance = 0;
        }
        else
        {
            stance = 1;
        }

        animator.SetInteger("stance", stance);
    }

    void OnMoveModifier(InputValue value)
    {
    }

    public ISet<string> AnimationClips()
    {
        ISet<string> result = new HashSet<string>();
        AnimatorClipInfo[] clips =
            animator.GetCurrentAnimatorClipInfo(gameObject.layer);
        foreach (AnimatorClipInfo info in clips)
        {
            foreach (string clip in info.clip.name.Split("_"))
            {
                result.Add(clip);
            }
        }
        return result;
    }

    public void OnAttackWeapon()
    {
        mode = AttackMode.WEAPON;
    }

    public void OnAttackNone()
    {
        mode = AttackMode.NONE;
    }

    public void OnAttackHand()
    {
        mode = AttackMode.HAND;
    }

    private enum AttackMode
    {
        NONE, HAND, WEAPON
    }
}
