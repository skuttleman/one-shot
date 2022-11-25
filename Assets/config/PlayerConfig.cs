using System;
using System.Collections.Generic;
using Game.System.Events.Player;
using UnityEngine;
using Game.Utils;

[CreateAssetMenu(menuName = "characters/PlayerConfig")]
public class PlayerConfig : ScriptableObject {
    [Header("Walking")]
    [SerializeField] float walkSpeed;
    [SerializeField] float maxWalkVelocity;
    [SerializeField] float walkRotationSpeed;
    [SerializeField] float maxStandAnimSpeed;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float maxCrouchVelocity;
    [SerializeField] float crouchRotationSpeed;
    [SerializeField] float maxCrouchAnimSpeed;

    [Header("Crawling")]
    [SerializeField] float crawlSpeed;
    [SerializeField] float maxCrawlelocity;
    [SerializeField] float crawlRotationSpeed;
    [SerializeField] float maxCrawlAnimSpeed;

    [Header("Misc")]
    [SerializeField] float rotationTolerance;
    [SerializeField] float aimFactor;
    [SerializeField] float scopeFactor;
    [SerializeField] float stanceChangeButtonHeldDuration = 0.35f;

    public float MovementForce(
        PlayerStance stance, Vector2 movement, bool isAiming, bool isScoping) {
        float speed = CaseStance(stance, walkSpeed, crouchSpeed, crawlSpeed);

        if (isAiming) speed *= aimFactor;
        else if (isScoping) speed *= scopeFactor;
        return movement.Max() * speed;
    }

    public float AnimationSpeed(PlayerStance stance, Vector2 movement) {
        float limit = CaseStance(stance,
            maxStandAnimSpeed, maxCrouchAnimSpeed, maxCrawlAnimSpeed);
        return Mathf.Lerp(0f, limit, movement.Max());
    }

    public Vector3 LimitVelocity(PlayerStance stance, Vector3 velocity) {
        float maxSpeed = CaseStance(stance,
            maxWalkVelocity, maxCrouchVelocity, maxCrawlelocity);
        if (velocity.magnitude > maxSpeed)
            return velocity.normalized * maxSpeed;
        return velocity;
    }

    public Vector3 RotationTorque(PlayerStance stance, float curr, float next) {
        return CaseStance(stance,
            walkRotationSpeed, crouchRotationSpeed, crawlRotationSpeed)
            * Time.fixedDeltaTime
            * RotationDirection((curr + 360) % 360, (next + 360) % 360);
    }

    public PlayerStance NextStance(PlayerStance stance, float stanceDuration) {
        bool held = stanceDuration >= stanceChangeButtonHeldDuration;

        if (held && stance == PlayerStance.CRAWLING)
            return PlayerStance.STANDING;
        else if (held)
            return PlayerStance.CRAWLING;
        else if (stance == PlayerStance.CROUCHING)
            return PlayerStance.STANDING;
        return PlayerStance.CROUCHING;
    }

    private Vector3 RotationDirection(float oldRot, float newRot) {
        float difference = Mathf.Abs(oldRot - newRot);

        if (difference < rotationTolerance) return Vector3.zero;
        else if (oldRot < newRot && difference >= 180) return Vector3.back;
        else if (oldRot < newRot) return Vector3.forward;
        else if (oldRot > newRot && difference >= 180) return Vector3.forward;
        return Vector3.back;
    }

    private T CaseStance<T>(PlayerStance stance, T onStand, T onCrouch, T onCrawl) {
        switch (stance) {
            case PlayerStance.STANDING:
                return onStand;
            case PlayerStance.CROUCHING:
                return onCrouch;
            default:
                return onCrawl;
        }
    }
}
