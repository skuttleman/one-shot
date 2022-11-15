using Game.System.Events.Player;
using Game.Utils;
using UnityEngine;

public class CameraController : Monos.Subscriber<
    ScopeChange, MovementSpeedChange, AttackModeChange>
{
    [Header("Player")]
    [SerializeField] Transform target;

    [Header("Camera Config")]
    [SerializeField] float rotateSpeed;
    [SerializeField] float moveOffset;
    [SerializeField] float binoOffset;
    [SerializeField] float aimOffset;
    [SerializeField] float maxLookAhead;

    CinemachineCameraOffset offset;
    bool isScoping;
    bool isMoving;
    bool isAiming;

    void Start()
    {
        Init();
        offset = GetComponent<CinemachineCameraOffset>();
    }

    void Update()
    {
        SetOffset();
    }

    void SetOffset()
    {
        float lookAhead = 0f;
        if (isScoping) lookAhead += binoOffset;
        else if (isMoving) lookAhead += moveOffset;
        if (isAiming) lookAhead += aimOffset;

        lookAhead = Mathf.Clamp(lookAhead, 0f, maxLookAhead);
        offset.m_Offset = Vector3.Lerp(
            offset.m_Offset,
            target.rotation * new Vector3(0, lookAhead, 0),
            rotateSpeed * Time.deltaTime);
    }

    public override void OnEvent(ScopeChange e) => isScoping = e.isScoping;
    public override void OnEvent(MovementSpeedChange e) =>
        isMoving = Maths.NonZero(e.speed);
    public override void OnEvent(AttackModeChange e) =>
        isAiming = e.mode == AttackModeChange.AttackMode.WEAPON;
    void OnDestroy() => Destroy();
}
