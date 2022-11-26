using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform focus;
    [SerializeField] float distanceZ;
    [SerializeField] float customFactor = 1f;
    [SerializeField] Vector2 maxOffset;

    private Vector3 realPosition;
    private Vector2 offset;

    public void UpdatePosition(Func<Vector3, Vector3> fn) {
        realPosition = fn(realPosition);
        UpdatePosition();
    }

    void Start() {
        realPosition = transform.position;
    }

    void Update()
    {
        offset = customFactor * distanceZ * (focus.position - transform.position).Downgrade();

        UpdatePosition();
    }


    void UpdatePosition() {
        transform.position = realPosition
            + Vectors.Clamp(offset.Upgrade(), -maxOffset, maxOffset);
    }
}
