using System;
using System.Collections.Generic;
using Game.System.Events.Player;
using UnityEngine;
using Game.Utils;

[CreateAssetMenu(menuName = "characters/PlayerConfig")]
public class PlayerCoreConfig : ScriptableObject {
    [Header("Movement")]
    [SerializeField] MoveConfig standing;
    [SerializeField] MoveConfig crouching;
    [SerializeField] MoveConfig crawling;

    [Header("Misc")]
    [SerializeField] float aimFactor;
    [SerializeField] float scopeFactor;
    [SerializeField] float stanceChangeButtonHeldThreshold;
    [SerializeField] float torqueThreshold;

    public float MovementForce(
        PlayerStance stance, Vector2 movement, bool isAiming, bool isScoping) {
        float speed = CaseStance(stance).moveSpeed;

        if (isAiming) speed *= aimFactor;
        else if (isScoping) speed *= scopeFactor;
        return movement.magnitude * speed;
    }

    public float AnimationSpeed(PlayerStance stance, Vector2 movement) {
        float limit = CaseStance(stance).maxAnimSpeed;
        return Mathf.Lerp(0f, limit, movement.magnitude);
    }

    public Vector3 LimitVelocity(PlayerStance stance, Vector3 velocity) {
        float maxSpeed = CaseStance(stance).maxVelocity;
        if (velocity.magnitude > maxSpeed)
            return velocity.normalized * maxSpeed;
        return velocity;
    }

    public Vector3 RotationTorque(PlayerStance stance, float curr, float next) {
        float angle = SimplifiedAngle(curr, next);
        float speed = CaseStance(stance).rotationSpeed;
        Vector3 direction = angle < 0 ? Vector3.back : Vector3.forward;
        if (Mathf.Abs(angle) <= torqueThreshold) return Vector3.zero;
        speed = Mathf.Clamp(0, speed, Mathf.Abs(angle) / 360);

        return speed * Time.fixedDeltaTime * direction;
    }

    private float SimplifiedAngle(float current, float next) {
        float currVal = (current + 360) % 360;
        float nextVal = (next + 360) % 360;
        float result = nextVal - currVal;
        float mod = Mathf.Sign(result) * 360;
        if (Mathf.Abs(result) > 180f) return -(mod - (result + mod) % mod);
        return result;
    }

    public PlayerStance NextStance(PlayerStance stance, float stanceDuration) {
        bool held = stanceDuration >= stanceChangeButtonHeldThreshold;

        if (held && stance == PlayerStance.CRAWLING)
            return PlayerStance.STANDING;
        else if (held)
            return PlayerStance.CRAWLING;
        else if (stance == PlayerStance.CROUCHING)
            return PlayerStance.STANDING;
        return PlayerStance.CROUCHING;
    }

    private MoveConfig CaseStance(PlayerStance stance) {
        switch (stance) {
            case PlayerStance.STANDING:
                return standing;
            case PlayerStance.CROUCHING:
                return crouching;
            case PlayerStance.CRAWLING:
                return crawling;
            default:
                return default;
        }
    }

    [Serializable]
    struct MoveConfig {
        public float moveSpeed;
        public float rotationSpeed;
        public float maxAnimSpeed;
        public float maxVelocity;
    }
}
