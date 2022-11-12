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
    Vector2 movement = Vector2.zero;
    GameSession session;

    // animation state
    int stance = 0;
    bool isMoving = false;
    bool isScoping = false;
    bool isAiming = false;
    bool isLooking = false;

    // movement modifiers
    [SerializeField] float walkSpeed = 2.5f;
    [SerializeField] float crouchSpeed = 1.5f;
    [SerializeField] float crawlSpeed = 0.75f;
    [SerializeField] float rotationSpeed = 8f;
    float rotationZ = 0f;
    float movementModifer = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        session = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        if (isMoving && !isLooking) rotationZ = Vectors.Angle(Vector2.zero, movement);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);

        float speed = movementModifer;
        switch (stance)
        {
            case 0:
                speed = walkSpeed;
                break;
            case 1:
                speed = crouchSpeed;
                break;
            case 2:
                speed = crawlSpeed;
                break;
        }
        if (isAiming) speed *= 0.9f;
        else if (isScoping) speed *= 0.6f;
        animator.speed = Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.y)) * speed;
        transform.position += speed * Time.deltaTime * Vectors.Upgrade(movement);
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
        if (value.isPressed)
        {
            movementModifer = 0.5f;
        } else
        {
            movementModifer = 1f;
        }
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

    public float Speed()
    {
        return animator.speed;
    }
}
