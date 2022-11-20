using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    public IEnumerator<YieldInstruction> Go(float magnitude)
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius *= magnitude;
        coll.enabled = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
