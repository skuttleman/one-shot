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
    PlayerMovement movement;

    void Start()
    {
        offset = GetComponent<CinemachineCameraOffset>();
        movement = target.gameObject.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        SetOffset();
    }

    void SetOffset()
    {
        float lookAhead = 0f;
        ISet<string> clips = movement.AnimationClips();

        if (clips.Contains("bino") || clips.Contains("tobino"))
        {
            lookAhead += binoOffset;
        }
        else if (clips.Contains("move"))
        {
            lookAhead += moveOffset;
        }
        if (clips.Contains("aim") || clips.Contains("toaim") || clips.Contains("fire"))
        {
            lookAhead += aimOffset;
        }

        lookAhead = Mathf.Clamp(lookAhead, 0f, maxLookAhead);

        offset.m_Offset = Vector3.Lerp(
            offset.m_Offset,
            target.rotation * new Vector3(0, lookAhead, 0),
            rotateSpeed * Time.deltaTime);
    }
}
