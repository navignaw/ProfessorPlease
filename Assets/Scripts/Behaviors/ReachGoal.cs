using UnityEngine;
using System.Collections;

/**
 * Move towards a target goal position.
 */
public class ReachGoal : BaseBehavior {
    public float stopRadius = 3f;
	public GameObject target;

    public override Vector3 ComputeVelocity() {
        // If close enough to target, stop moving
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        if (distance <= stopRadius) {
            return Vector3.zero;
        }

        float scale = 1f;

        // If approaching 10 * radius, start slowing down
        if (distance <= 10 * stopRadius) {
            scale *= Mathf.Max(0.25f, (distance - stopRadius) / (10 * stopRadius));
        }

    	Vector3 rot = target.transform.rotation.eulerAngles;
        Quaternion qrot = Quaternion.Euler(rot.x, rot.y, rot.z);
        Vector3 aim = qrot * Vector3.zero;
    	Vector3 v = (target.transform.position + aim - this.transform.position) * scale;
        return v;
    }

}