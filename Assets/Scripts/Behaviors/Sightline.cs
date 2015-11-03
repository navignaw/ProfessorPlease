using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PriorityQueue;

/**
 * Move towards a target goal position via A* pathfinding magic.
 */
public class Sightline : BaseBehavior {
	public GameObject target;
    public float distance;
    public BaseBehavior pathfind;

    public override Vector3 ComputeVelocity() {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(this.transform.position, target.transform.position - this.transform.position, out hit, distance) && hit.collider.tag == "Player") {
            pathfind.scale = 0.6f;
        }
        return Vector3.zero;
    }

    // Is the position ahead of me?
    private bool IsAhead(Vector3 pos) {
        float angle = Mathf.Abs(Vector3.Angle(this.transform.forward, transform.position - pos));
        return (angle > 90 || angle < 270);
    }

    void OnDrawGizmos() {

    }

}