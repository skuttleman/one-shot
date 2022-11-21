using System;
using Game.System.AI.Interfaces.Patrol;
using Game.System.AI.Patrol;
using Game.System.AI.Patrol.Components;
using Game.System.AI.Patrol.Composables;
using Game.Utils;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [Header("Patrol")]
    [SerializeField] float patrolSpeed = 0.25f;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] UIPatrolStep[] patrolSteps;

    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        IPatrol patrol = PatrolCycle.Of(Sequences.Map(patrolSteps, GeneratePatrol));
        StartCoroutine(APatrol.DoPatrol(patrol, transform));
    }

    IPatrol GeneratePatrol(UIPatrolStep step) {
        IPatrol moveLook;

        if (step.isLook) {
            moveLook = new PatrolLookAt(transform, step.position, rotationSpeed);
        } else {
            moveLook = new Patrol(
                new PatrolStep(new PatrolAction(() => anim.SetBool("isMoving", true))),
                new PatrolWalkTo(transform, step.position, patrolSpeed, rotationSpeed),
                new PatrolStep(new PatrolAction(() => anim.SetBool("isMoving", false))));
        }

        IPatrolStep wait = new PatrolYield(new WaitForSeconds(step.waitAfter));
        if (step.waitAfter > 0)
            return new Patrol(moveLook, new PatrolStep(wait));
        return moveLook;
    }

    [Serializable]
    public record UIPatrolStep {
        [SerializeField] public Vector3 position;
        [SerializeField] public float waitAfter;
        [SerializeField] public bool isLook;
    }
}
