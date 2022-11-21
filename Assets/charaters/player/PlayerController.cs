using UnityEngine;
using Game.Utils;
using Game.System.Events.Player;
using Game.Utils.Mono;
using Game.System.Events;
using System.Threading;
using System;

public class PlayerController : Subscriber
    <Event<PlayerStance>, Event<PlayerAttackMode>,
     PlayerMovementSpeedChange, PlayerScopeChange> {
    Animator animator;
    Vector2 movement = Vector2.zero;

    // animation state
    PlayerAttackMode mode;
    PlayerStance stance;
    bool isMoving;
    bool isScoping;

    // movement modifiers
    [SerializeField] float walkSpeed = 2.5f;
    [SerializeField] float crouchSpeed = 1.5f;
    [SerializeField] float crawlSpeed = 0.75f;
    [SerializeField] float rotationSpeed = 8f;
    float rotationZ = 0f;
    float movementModifer = 1f;

    new void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    new void Update() {
        base.Update();
        RotatePlayer();
        MovePlayer();
    }

    void RotatePlayer() {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);
    }

    void MovePlayer() {
        if (IsMovable()) {
            float speed = movementModifer * StanceSpeed();

            if (IsAiming()) speed *= 0.9f;
            else if (isScoping) speed *= 0.6f;
            float movementSpeed = Mathf.Max(
                Mathf.Abs(movement.x),
                Mathf.Abs(movement.y));
            animator.speed = movementSpeed * speed;
            transform.position += speed * Time.deltaTime * Vectors.Upgrade(movement);
        }
    }

    float StanceSpeed() {
        if (IsCrouching()) return crouchSpeed;
        if (IsCrawling()) return crawlSpeed;
        return walkSpeed;
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

    public void InputMoveModified(bool isModified) =>
        movementModifer = isModified ? 0.5f : 1f;
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
