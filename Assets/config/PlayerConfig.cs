using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "characters/PlayerConfig")]
public class PlayerConfig : ScriptableObject {
    [Header("Walking")]
    public float walkSpeed = 2.5f;
    public float walkRotationSpeed = 1f;
    [Header("Crouching")]
    public float crouchSpeed = 1.5f;
    public float crouchRotationSpeed = 1f;
    [Header("Crawling")]
    public float crawlSpeed = 0.75f;
    public float crawlRotationSpeed = 1f;

    [SerializeField] float rotationTolerance = 1f;

    public Vector3 RotationDirection(float oldRot, float newRot) {
        float difference = Mathf.Abs(oldRot - newRot);

        if (difference < rotationTolerance) return Vector3.zero;
        else if (oldRot < newRot && difference >= 180) return Vector3.back;
        else if (oldRot < newRot) return Vector3.forward;
        else if (oldRot > newRot && difference >= 180) return Vector3.forward;
        return Vector3.back;
    }
}
