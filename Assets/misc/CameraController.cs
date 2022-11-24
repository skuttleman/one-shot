using Game.System.Events.Player;
using Game.Utils;
using UnityEngine;
using Game.Utils.Mono;
using Game.System.Events;
using System;

public class CameraController
    : Subscriber<PlayerScopeChange, PlayerMovementSpeedChange, Event<PlayerAttackMode>> {
    [Header("Camera Config")]
    [SerializeField] float rotateSpeed;
    [SerializeField] float moveOffset;
    [SerializeField] float binoOffset;
    [SerializeField] float aimOffset;
    [SerializeField] float maxLookAhead;
    [SerializeField] string targetTag;

    GameSession session;
    Transform target;
    CinemachineCameraOffset offset;
    bool isScoping;
    bool isMoving;
    bool isAiming;

    new void Start() {
        base.Start();
        offset = GetComponent<CinemachineCameraOffset>();
        session = FindObjectOfType<GameSession>();
        target = session.GetTaggedObject(targetTag).transform;
    }

    new void Update() {
        base.Update();
        SetOffset();
    }

    void SetOffset() {
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

    public override void OnEvent(PlayerScopeChange e) => isScoping = e.data;
    public override void OnEvent(PlayerMovementSpeedChange e) =>
        isMoving = Maths.NonZero(e.data);
    public override void OnEvent(Event<PlayerAttackMode> e) =>
        isAiming = e.data == PlayerAttackMode.WEAPON
            || e.data == PlayerAttackMode.FIRING;
}
