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
    AnimationListener anim;

    void Start()
    {
        offset = GetComponent<CinemachineCameraOffset>();
        anim = target.gameObject.GetComponent<AnimationListener>();
    }

    void Update()
    {
        SetOffset();
    }

    void SetOffset()
    {
        float lookAhead = 0f;
        ISet<string> clips = anim.AnimationClips();

        if (Sets.ContainsAny(clips, "bino", "tobino"))
        {
            lookAhead += binoOffset;
        }
        else if (clips.Contains("move"))
        {
            lookAhead += moveOffset;
        }
        if (Sets.ContainsAny(clips, "aim", "toaim", "fire"))
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
