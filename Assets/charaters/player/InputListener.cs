using Game.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void OnAim(InputValue value) =>
        controller.InputAim(Maths.NonZero(value.Get<float>()));

    void OnAttack(InputValue value) =>
        controller.InputAttack(value.isPressed);

    void OnLook(InputValue value) =>
        controller.InputLook(value.Get<Vector2>());

    void OnMove(InputValue value) =>
        controller.InputMove(value.Get<Vector2>());

    void OnBinoculars(InputValue value) =>
        controller.InputScope(value.isPressed);

    void OnStance(InputValue value) =>
        controller.InputStance(value.Get<float>());

    void OnMoveModifier(InputValue value) =>
        controller.InputMoveModified(value.isPressed);
}
