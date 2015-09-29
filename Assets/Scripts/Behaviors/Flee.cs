using UnityEngine;
using System.Collections;

/**
 * Move away from an enemy pursuer.
 */
public class Flee : BaseBehavior {
    public float stopRadius = 15f;
	public GameObject target;
    public BaseBehavior idleBehavior;

    public override Vector3 ComputeVelocity() {
        // If far enough from target, switch to idle behavior
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        if (distance > stopRadius) {
            return idleBehavior.ComputeVelocity();
        }

        float scale = 1f;

        // If approaching radius, start slowing down
        if (distance > 0.5f * stopRadius) {
            scale *= Mathf.Max(0.25f, (stopRadius - distance) / (0.5f * stopRadius));
        }

        // Otherwise, move in opposite direction of seek
    	return (this.transform.position - target.transform.position) * scale;
    }

}