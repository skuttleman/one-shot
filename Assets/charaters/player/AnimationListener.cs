using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    AttackMode mode = AttackMode.HAND;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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

    public ISet<string> AnimationClips()
    {
        return Colls.Reduce(
            animator.GetCurrentAnimatorClipInfo(gameObject.layer),
            (acc, item) => Colls.Reduce(
                item.clip.name.Split("_"),
                (acc, name) => Colls.Add(acc, name),
                acc),
            new HashSet<string>());
    }

    public float Speed()
    {
        return animator.speed;
    }
}

public enum AttackMode
{
    NONE, HAND, WEAPON
}