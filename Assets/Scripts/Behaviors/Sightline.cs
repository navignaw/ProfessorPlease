using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Move towards a target goal position via A* pathfinding magic.
 */
public class Sightline : BaseBehavior {
    public float distance;
    public BaseBehavior pathfind;

    private int lastUpdatedTarget = 20;

    public override Vector3 ComputeVelocity() {
        if (target == null) {
            return Vector3.zero;
        }

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(this.transform.position, target.transform.position - this.transform.position, out hit, distance) && hit.collider.tag == "Player"
            && IsAhead(target.transform.position)) {
            pathfind.scale = 0.6f;
            this.scale = 1f;
        }
        else {
            this.scale = 0.1f;
        }
        return Vector3.zero;
    }

    // Is the position ahead of me?
    private bool IsAhead(Vector3 pos) {
        float angle = Mathf.Abs(Vector3.Angle(this.transform.forward, transform.position - pos));
        return (angle > 90 || angle < 270);
    }

    void Update() {
        if (--lastUpdatedTarget == 0) {
            FindProfessorTarget();
            lastUpdatedTarget = 20;
        }
    }

}