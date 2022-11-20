using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] Vector3 position1;
    [SerializeField] Vector3 position2;
    [SerializeField] float patrolSpeed = 0.25f;
    [SerializeField] float rotationSpeed = 0.1f;

    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        StartCoroutine(Patrol());
    }

    IEnumerator<YieldInstruction> Patrol() {
        while (true) {
            foreach (YieldInstruction instruction in WalkTo(position1))
                yield return instruction;

            yield return new WaitForSeconds(3);

            foreach (YieldInstruction instruction in WalkTo(position2))
                yield return instruction;

            yield return new WaitForSeconds(5);

            foreach (YieldInstruction instruction in LookAt(transform.position - new Vector3(0, 1, 0)))
                yield return instruction;

            foreach (YieldInstruction instruction in LookAt(transform.position + new Vector3(0, 1, 0)))
                yield return instruction;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerable<YieldInstruction> WalkTo(Vector3 position) {
        anim.SetBool("isMoving", true);
        transform.rotation.SetLookRotation(position);

        while (Maths.Distance(transform.position, position) > patrolSpeed) {
            Sequences.First(LookAt(position));
            transform.position += (position - transform.position).normalized * patrolSpeed;
            yield return new WaitForEndOfFrame();
        }
        anim.SetBool("isMoving", false);
    }

    IEnumerable<YieldInstruction> LookAt(Vector3 position) {
        float rotationZ = Vectors.AngleTo(transform.position, position);

        while (Mathf.Abs(transform.rotation.eulerAngles.z - rotationZ) >= rotationSpeed) {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, rotationZ),
                rotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
