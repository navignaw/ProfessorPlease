using UnityEngine;
using System.Collections;

/**
 * Move towards a target goal position.
 * @TODO: this is a copy of ReachGoal, actually implement pathfinding
 */
public class Pathfinding : BaseBehavior {
    public float stopRadius = 3f;
    public bool lookAhead = true;
	public GameObject target;
    private Rigidbody _targetRigidbody;

    private void Start() {
        _targetRigidbody = target.GetComponent<Rigidbody>();
    }

    public override Vector3 ComputeVelocity() {
        // If close enough to target, stop moving
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        if (distance <= stopRadius) {
            return Vector3.zero;
        }

        float scale = 1f;
        Vector3 targetPos = target.transform.position;

        // If approaching 2 * radius, start slowing down
        if (distance <= 2 * stopRadius) {
            scale *= Mathf.Max(0.25f, (distance - stopRadius) / (2 * stopRadius));
        }

        // If distant and lookAhead, dynamically pursue a bit ahead of the target
        else if (lookAhead) {
            targetPos += _targetRigidbody.velocity;
        }

    	return (targetPos - this.transform.position).normalized * scale;
    }

}