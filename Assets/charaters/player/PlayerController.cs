using UnityEngine;
using Game.Utils;
using Game.System.Events.Player;
using Game.Utils.Mono;

public class PlayerController :
    Subscriber<StanceChange, AttackModeChange, MovementSpeedChange, ScopeChange>
{
    Animator animator;
    Vector2 movement = Vector2.zero;

    // animation state
    AttackModeChange.AttackMode mode;
    StanceChange.Stance stance;
    bool isMoving;
    bool isScoping;

    // movement modifiers
    [SerializeField] float walkSpeed = 2.5f;
    [SerializeField] float crouchSpeed = 1.5f;
    [SerializeField] float crawlSpeed = 0.75f;
    [SerializeField] float rotationSpeed = 8f;
    float rotationZ = 0f;
    float movementModifer = 1f;

    new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    new void Update()
    {
        base.Update();
        RotatePlayer();
        MovePlayer();
    }

    void RotatePlayer()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, rotationZ),
            rotationSpeed * Time.deltaTime);
    }

    void MovePlayer()
    {
        if (IsMovable())
        {
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

    float StanceSpeed()
    {
        if (IsCrouching()) return crouchSpeed;
        if (IsCrawling()) return crawlSpeed;
        return walkSpeed;
    }

    public void InputAttack(bool isAttacking)
    {
        if (isAttacking && CanAttack()) animator.SetTrigger("attack");
    }

    public void InputLook(Vector2 direction)
    {
        if (Vectors.NonZero(direction))
            rotationZ = Vectors.AngleTo(Vector2.zero, direction);
    }

    public void InputMove(Vector2 direction)
    {
        movement = direction;
        bool isMoving = Vectors.NonZero(movement);
        if (isMoving) rotationZ = Vectors.AngleTo(Vector2.zero, movement);
        animator.SetBool("isMoving", isMoving);
    }

    public void InputStance(float value)
    {
        bool held = value >= 0.35f;
        StanceChange.Stance nextStance;

        if (held && IsCrawling()) nextStance = StanceChange.Stance.STANDING;
        else if (held) nextStance = StanceChange.Stance.CRAWLING;
        else if (IsCrouching()) nextStance = StanceChange.Stance.STANDING;
        else nextStance = StanceChange.Stance.CROUCHING;

        if (nextStance != StanceChange.Stance.CRAWLING
            || (!IsAiming() && !isScoping)
            || !isMoving)
        {
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
    bool IsCrawling() => stance == StanceChange.Stance.CRAWLING;
    bool IsCrouching() => stance == StanceChange.Stance.CROUCHING;
    bool IsAiming() => mode == AttackModeChange.AttackMode.WEAPON
        || mode == AttackModeChange.AttackMode.FIRING;
    bool IsMovable() => !IsCrawling() || (!IsAiming() && !isScoping);
    public override void OnEvent(ScopeChange e) => isScoping = e.isScoping;
    public override void OnEvent(StanceChange e) => stance = e.stance;
    public override void OnEvent(AttackModeChange e) => mode = e.mode;
    public override void OnEvent(MovementSpeedChange e) =>
        isMoving = Maths.NonZero(e.speed);

    private bool CanAttack() =>
        mode != AttackModeChange.AttackMode.NONE
            && mode != AttackModeChange.AttackMode.FIRING
            && mode != AttackModeChange.AttackMode.PUNCHING;
}
