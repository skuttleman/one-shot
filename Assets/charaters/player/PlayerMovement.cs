using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Utils;

public class PlayerMovement : MonoBehaviour
{
    static readonly float MOVE_SPEED = 850f;

    Animator animator;
    float rotationSpeed = 8f;
    float rotationZ = 0f;
    Vector2 movement = Vector2.zero;
    Rigidbody rigidBody;
    bool isMoving = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);
        Vector3 force = Math.upgrade(movement) * MOVE_SPEED * Time.deltaTime;
        rigidBody.AddForce(force);
    }

    void OnAim(InputValue value)
    {
    }

    void OnAttack(InputValue value)
    {
    }

    void OnLook(InputValue value)
    {
        Vector2 rotation = value.Get<Vector2>();
        bool isRotating = rotation != Vector2.zero;
        if (isRotating) rotationZ = Math.Angle(Vector2.zero, rotation);
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
        if (isMoving) rotationZ = Math.Angle(Vector2.zero, movement);
    }

    void OnBinoculars(InputValue value)
    {
    }

    void OnStance(InputValue value)
    {
        bool held = value.Get<float>() >= 0.5f;
        bool isCrouching = animator.GetBool("isCrouching");
        bool isCrawling = animator.GetBool("isCrawling");

        if (held && isCrawling)
        {
            isCrouching = false;
            isCrawling = false;
        }
        else if (held)
        {
            isCrouching = false;
            isCrawling = true;
        }
        else if (isCrouching)
        {
            isCrouching = false;
            isCrawling = false;
        }
        else
        {
            isCrouching = true;
            isCrawling = false;
        }

        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isCrawling", isCrawling);
    }

    void OnMoveModifier(InputValue value)
    {
    }
}
