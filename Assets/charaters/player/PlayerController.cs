using UnityEngine;
using Game.Utils;
using Game.System.Events.Player;
using Game.Utils.Mono;
using Game.System.Events;
using System.Collections.Generic;

public class PlayerController : Subscriber
    <Event<PlayerStance>, Event<PlayerAttackMode>,
     PlayerMovementSpeedChange, PlayerScopeChange> {
    private static readonly string ATTACK_TRIGGER = "attack";
    private static readonly string MOVE_BOOL = "isMoving";
    private static readonly string AIM_BOOL = "isAiming";
    private static readonly string SCOPE_BOOL = "isScoping";
    private static readonly string STANCE_INT = "stance";


    [SerializeField] PlayerCoreConfig cfg;
    Animator animator;
    Rigidbody rb;

    // movement state
    PlayerAttackMode mode;
    PlayerStance stance;
    Vector2 movement = Vector2.zero;
    Vector2 facing = Vector2.zero;
    bool isMoving;
    bool isScoping;
    bool isLooking;

    new void Start() {
        base.Start();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void RotatePlayer() {
        float rotationZ;
        if (isLooking) rotationZ = Vectors.AngleTo(Vector2.zero, facing);
        else if (Vectors.NonZero(movement)) rotationZ = Vectors.AngleTo(Vector2.zero, movement);
        else return;

        Vector3 torque = cfg.RotationTorque(
            stance,
            rb.rotation.eulerAngles.z,
            rotationZ);
        if (torque != Vector3.zero)
            rb.AddRelativeTorque(torque);
    }

    void MovePlayer() {
        if (IsMovable(stance)) {
            animator.speed = cfg.AnimationSpeed(stance, movement);
            float force = cfg.MovementForce(
                stance,
                movement,
                IsAiming(),
                isScoping);
            if (Maths.NonZero(force))
                rb.AddForce(movement.normalized * force);
        }
    }

    void FixedUpdate() {
        RotatePlayer();
        MovePlayer();
        rb.velocity = cfg.LimitVelocity(stance, rb.velocity);
    }

    public void InputAttack(bool isAttacking) {
        if (isAttacking && CanAttack()) animator.SetTrigger(ATTACK_TRIGGER);
    }

    public void InputLook(Vector2 direction) {
        facing = direction;
        isLooking = Vectors.NonZero(facing);
    }

    public void InputMove(Vector2 direction) {
        movement = direction;
        animator.SetBool(MOVE_BOOL, Vectors.NonZero(movement));
    }

    public void InputStance(float value) {
        PlayerStance nextStance = cfg.NextStance(stance, value);

        if (IsMovable(nextStance) || !isMoving) {
            stance = nextStance;
            animator.SetInteger(STANCE_INT, (int)stance);
        }
    }

    public void InputMoveModified(bool _) { }
    public void InputAim(bool isAiming) =>
        animator.SetBool(AIM_BOOL, isAiming);
    public void InputScope(bool isScoping) =>
        animator.SetBool(SCOPE_BOOL, isScoping);

    public override void OnEvent(PlayerScopeChange e) => isScoping = e.data;
    public override void OnEvent(Event<PlayerStance> e) => stance = e.data;
    public override void OnEvent(Event<PlayerAttackMode> e) => mode = e.data;
    public override void OnEvent(PlayerMovementSpeedChange e) =>
        isMoving = Maths.NonZero(e.data);

    bool IsAiming() =>
        mode == PlayerAttackMode.WEAPON || mode == PlayerAttackMode.FIRING;
    bool IsMovable(PlayerStance stance) =>
        stance != PlayerStance.CRAWLING || (!IsAiming() && !isScoping);
    bool CanAttack() =>
        mode != PlayerAttackMode.NONE
            && mode != PlayerAttackMode.FIRING
            && mode != PlayerAttackMode.PUNCHING;
}
