using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Utils;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] static float MOVE_SPEED = 850f;

    Animator animator;
    float rotationSpeed = 8f;
    float rotationZ = 0f;
    Vector2 movement = Vector2.zero;

    // animation state
    bool isMoving = false;
    bool isScoping = false;
    bool isAiming = false;
    bool isLooking = false;
    bool isCrawling = false;
    bool isStanding = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);
        transform.position += Vectors.Upgrade(movement) * Time.deltaTime;
    }

    void OnAim(InputValue value)
    {
        if (!isCrawling || !isMoving)
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
        if (!isCrawling || (!isAiming && !isScoping))
        {
            movement = value.Get<Vector2>();
            isMoving = movement != Vector2.zero;
            animator.SetBool("isMoving", isMoving);
            rotationZ = Vectors.Angle(Vector2.zero, movement);
        }
        else if (movement != Vector2.zero && isCrawling && (isScoping || isAiming))
        {
            rotationZ = Vectors.Angle(Vector2.zero, value.Get<Vector2>());
        }
    }

    void OnBinoculars(InputValue value)
    {
        if (!isCrawling || !isMoving)
        {
            isScoping = value.isPressed;
            animator.SetBool("isScoping", isScoping);
        }
    }

    void OnStance(InputValue value)
    {
        bool held = value.Get<float>() >= 0.5f;
        isStanding = animator.GetBool("isStanding");
        isCrawling = animator.GetBool("isCrawling");

        if (held && isCrawling)
        {
            isStanding = true;
            isCrawling = false;
        }
        else if (held)
        {
            isStanding = false;
            isCrawling = true;
            if (isMoving)
            {
                isAiming = false;
                isScoping = false;
                animator.SetBool("isAiming", isAiming);
                animator.SetBool("isScoping", isScoping);
            }
        }
        else if (!isCrawling && !isStanding)
        {
            isStanding = true;
            isCrawling = false;
        }
        else
        {
            isStanding = false;
            isCrawling = false;
        }

        animator.SetBool("isStanding", isStanding);
        animator.SetBool("isCrawling", isCrawling);
    }

    void OnMoveModifier(InputValue value)
    {
    }
}
