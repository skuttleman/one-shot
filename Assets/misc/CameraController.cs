using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game.Utils;
using UnityEngine;

public class CameraController : MonoBehaviour
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
    PlayerAnimationStateListener anim;

    void Start()
    {
        offset = GetComponent<CinemachineCameraOffset>();
        anim = target.gameObject.GetComponent<PlayerAnimationStateListener>();
    }

    void Update()
    {
        SetOffset();
    }

    void SetOffset()
    {
        float lookAhead = 0f;
        if (anim.IsScoping()) lookAhead += binoOffset;
        else if (anim.IsMoving()) lookAhead += moveOffset;
        if (anim.IsAiming()) lookAhead += aimOffset;

        lookAhead = Mathf.Clamp(lookAhead, 0f, maxLookAhead);

        offset.m_Offset = Vector3.Lerp(
            offset.m_Offset,
            target.rotation * new Vector3(0, lookAhead, 0),
            rotateSpeed * Time.deltaTime);
    }
}
