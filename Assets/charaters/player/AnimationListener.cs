using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    AttackMode mode = AttackMode.HAND;

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

}

public enum AttackMode
{
    NONE, HAND, WEAPON
}