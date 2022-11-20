using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour {
    public IEnumerator<YieldInstruction> Go(float magnitude) {
        SphereCollider coll = GetComponent<SphereCollider>();
        Renderer rdr = GetComponent<Renderer>();
        if (coll) {
            coll.radius *= magnitude;
            coll.enabled = true;
        }
        if (rdr) rdr.enabled = true;

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
