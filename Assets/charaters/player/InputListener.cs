using System.Collections;
using System.Collections.Generic;
using Game.System;
using Game.System.Events.Player;
using Game.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    IPubSub pubsub;

    void Start()
    {
        pubsub = FindObjectOfType<GameSession>().Get<IPubSub>();
    }

    void OnAim(InputValue value) =>
        pubsub.PublishSync(new InputAim(Maths.NonZero(value.Get<float>())));

    void OnAttack(InputValue value) =>
        pubsub.PublishSync(new InputAttack(value.isPressed));

    void OnLook(InputValue value) =>
        pubsub.PublishSync(new InputLook(value.Get<Vector2>()));

    void OnMove(InputValue value) =>
        pubsub.PublishSync(new InputMove(value.Get<Vector2>()));

    void OnBinoculars(InputValue value) =>
        pubsub.PublishSync(new InputScope(value.isPressed));

    void OnStance(InputValue value) =>
        pubsub.PublishSync(new InputStance(value.Get<float>()));

    void OnMoveModifier(InputValue value) =>
        pubsub.PublishSync(new InputMoveModified(value.isPressed));
}
