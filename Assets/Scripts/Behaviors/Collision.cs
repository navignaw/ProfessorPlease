using UnityEngine;
using System.Collections;

/**
 * Collision detection. Fire rays in a cone (field of vision) and
 * check for collisions with GameObjects tagged "Wall"
 */
public class Collision : BaseBehavior {
    public float dist;
    private int lastChanged = 0;

    public override Vector3 ComputeVelocity() {

        Quaternion qRot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Quaternion fRot = Quaternion.Euler(0.0f, 10.0f, 0.0f);
        Quaternion fRot2 = Quaternion.Euler(0.0f, -10.0f, 0.0f);
        Quaternion lRot = Quaternion.Euler(0.0f, -45.0f, 0.0f);
        Vector3 collisionForce = Vector3.zero;
        RaycastHit hit = new RaycastHit();
        Vector3 forward1 = this.transform.forward;
        Vector3 forward2 = fRot * forward1;
        Vector3 forward3 = fRot * forward2;
        Vector3 forward4 = fRot2 * forward1;
        Vector3 forward5 = fRot2 * forward4;
        Vector3 newpos = this.transform.position;
        newpos.y += 0.8f;

        if (Physics.Raycast(newpos, forward1, out hit, dist) ||
            Physics.Raycast(newpos, forward2, out hit, dist) ||
            Physics.Raycast(newpos, forward3, out hit, dist) ||
            Physics.Raycast(newpos, forward4, out hit, dist) ||
            Physics.Raycast(newpos, forward5, out hit, dist)) {
            if (hit.collider.tag == "Wall") {
                collisionForce = qRot * forward1;
                lastChanged = 50;
            }
        } else if (lastChanged > 0) {
            lastChanged--;
            if (Physics.Raycast(newpos, lRot * forward1, out hit, dist + 3.0f) ||
                Physics.Raycast(newpos, lRot * (lRot *forward1), out hit, dist + 3.0f)) {
                return forward1;
            } else {
                lastChanged = 0;
            }
        }

        return collisionForce;
    }

}