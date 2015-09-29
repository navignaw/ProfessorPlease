using UnityEngine;
using System.Collections;

/**
 * Follow the leader in formation.
 */
public class FormationFollow : BaseBehavior {
    public Vector3 pos;
    public GameObject leader;
    private Vector3 aimahead = new Vector3(0.0f, 0.0f, 0.75f);

    /**
     * returns velocity toward assigned position, taking into account
     * the rotation of the leader and a need to aim slightly ahead of the
     * assigned position to maintain a clean formation
     */
    public override Vector3 ComputeVelocity() {
        Vector3 leaderRot = leader.transform.rotation.eulerAngles;
        Quaternion qRot = Quaternion.Euler(leaderRot.x, leaderRot.y, leaderRot.z);
        Vector3 aheadPos = pos + aimahead;
        Vector3 aim = qRot * aheadPos;
        Vector3 v = leader.transform.position + aim - this.transform.position;
        return v;
    }

}