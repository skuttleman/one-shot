using UnityEngine;
using Game.Utils;
using Game.System.Events.Player;
using Game.Utils.Mono;
using Game.System.Events;
using System.Collections.Generic;

public class PlayerController : Subscriber
    <Event<PlayerStance>, Event<PlayerAttackMode>,
     PlayerMovementSpeedChange, PlayerScopeChange> {
    [SerializeField] PlayerConfig cfg;
    Animator animator;
    Rigidbody rb;

    // movement state
    PlayerAttackMode mode;
    PlayerStance stance;
    Vector2 movement = Vector2.zero;
    float rotationZ = 0f;
    bool isMoving;
    bool isScoping;

    new void Start() {
        base.Start();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void RotatePlayer() {
        float currentRotation = (rb.rotation.eulerAngles.z + 360) % 360;
        rotationZ = (rotationZ + 360) % 360;

        rb.AddRelativeTorque(StanceRotationSpeed() * Time.fixedDeltaTime * cfg.RotationDirection(currentRotation, rotationZ));
    }

    void MovePlayer() {
        if (IsMovable()) {
            float speed = StanceMovementSpeed();

            if (IsAiming()) speed *= 0.9f;
            else if (isScoping) speed *= 0.6f;
            float movementSpeed = Mathf.Max(
                Mathf.Abs(movement.x),
                Mathf.Abs(movement.y));
            animator.speed = movementSpeed * speed;
            rb.AddRelativeForce(Vector3.up * animator.speed);
        }
    }

    void FixedUpdate() {
        RotatePlayer();
        MovePlayer();
    }

    float StanceMovementSpeed() {
        if (IsCrouching()) return cfg.crouchSpeed;
        if (IsCrawling()) return cfg.crawlSpeed;
        return cfg.walkSpeed;
    }

    float StanceRotationSpeed() {
        if (IsCrouching()) return cfg.crouchRotationSpeed;
        if (IsCrawling()) return cfg.crawlRotationSpeed;
        return cfg.walkRotationSpeed;
    }

    public void InputAttack(bool isAttacking) {
        if (isAttacking && CanAttack()) animator.SetTrigger("attack");
    }

    public void InputLook(Vector2 direction) {
        if (Vectors.NonZero(direction))
            rotationZ = Vectors.AngleTo(Vector2.zero, direction);
    }

    public void InputMove(Vector2 direction) {
        movement = direction;
        bool isMoving = Vectors.NonZero(movement);
        if (isMoving) rotationZ = Vectors.AngleTo(Vector2.zero, movement);
        animator.SetBool("isMoving", isMoving);
    }

    public void InputStance(float value) {
        bool held = value >= 0.35f;
        PlayerStance nextStance;

        if (held && IsCrawling()) nextStance = PlayerStance.STANDING;
        else if (held) nextStance = PlayerStance.CRAWLING;
        else if (IsCrouching()) nextStance = PlayerStance.STANDING;
        else nextStance = PlayerStance.CROUCHING;

        if (nextStance != PlayerStance.CRAWLING
            || (!IsAiming() && !isScoping)
            || !isMoving) {
            stance = nextStance;
            animator.SetInteger("stance", (int)stance);
        }
    }

    public void InputMoveModified(bool _) { }
    public void InputAim(bool isAiming) =>
        animator.SetBool("isAiming", isAiming);
    public void InputScope(bool isScoping) =>
        animator.SetBool("isScoping", isScoping);
    bool IsCrawling() => stance == PlayerStance.CRAWLING;
    bool IsCrouching() => stance == PlayerStance.CROUCHING;
    bool IsAiming() => mode == PlayerAttackMode.WEAPON
        || mode == PlayerAttackMode.FIRING;
    bool IsMovable() => !IsCrawling() || (!IsAiming() && !isScoping);
    public override void OnEvent(PlayerScopeChange e) => isScoping = e.data;
    public override void OnEvent(Event<PlayerStance> e) => stance = e.data;
    public override void OnEvent(Event<PlayerAttackMode> e) => mode = e.data;
    public override void OnEvent(PlayerMovementSpeedChange e) =>
        isMoving = Maths.NonZero(e.data);

    private bool CanAttack() =>
        mode != PlayerAttackMode.NONE
            && mode != PlayerAttackMode.FIRING
            && mode != PlayerAttackMode.PUNCHING;
}
