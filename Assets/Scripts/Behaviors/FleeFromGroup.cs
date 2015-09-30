using UnityEngine;
using System.Collections;

/**
 * Move away from multiple enemy pursuers.
 */
public class FleeFromGroup : BaseBehavior {
    public float stopRadius = 15f;
	public GameObject[] targets;
    public BaseBehavior idleBehavior;

    public override Vector3 ComputeVelocity() {
        Vector3 newVelocity = Vector3.zero;

        foreach (GameObject target in targets) {
            float distance = Vector3.Distance(target.transform.position, this.transform.position);
            if (distance > stopRadius) {
                continue;
            }

            float scale = 1f;

            // If approaching radius, start slowing down
            if (distance > 0.5f * stopRadius) {
                scale *= Mathf.Max(0.25f, (stopRadius - distance) / (0.5f * stopRadius));
            }

            newVelocity += (this.transform.position - target.transform.position) * scale;
        }

        // If far enough from all targets, switch to idle behavior
        if (newVelocity == Vector3.zero) {
            return idleBehavior.ComputeVelocity();
        }

        // Otherwise, move in combined direction
    	return newVelocity;
    }

}