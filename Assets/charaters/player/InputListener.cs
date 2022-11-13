using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void OnAim(InputValue value) => controller.Aim(value.Get<float>());

    void OnAttack(InputValue value) => controller.Attack(value.isPressed);

    void OnLook(InputValue value) => controller.Look(value.Get<Vector2>());

    void OnMove(InputValue value) => controller.Move(value.Get<Vector2>());

    void OnBinoculars(InputValue value) => controller.Binoculars(value.isPressed);

    void OnStance(InputValue value) => controller.Stance(value.Get<float>());

    void OnMoveModifier(InputValue value) => controller.MoveModifier(value.isPressed);
}
