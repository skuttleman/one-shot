using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    AttackMode mode = AttackMode.HAND;
    PlayerMovement movement;

    void Start()
    {
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    public AttackMode Mode() => mode;

    public void OnAttackWeapon()
    {
        mode = AttackMode.WEAPON;
    }

    public void OnAttackNone()
    {
        mode = AttackMode.NONE;
    }

    public void OnAttackHand()
    {
        mode = AttackMode.HAND;
    }

    public void OnStep()
    {
    }

}

public enum AttackMode
{
    NONE, HAND, WEAPON
}