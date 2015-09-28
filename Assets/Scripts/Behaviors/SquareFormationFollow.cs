using UnityEngine;
using System.Collections;

/**
 * Random wander behavior. TODO: Jun implement this
 */
public class SquareFormationFollow : BaseBehavior {
	public Vector3 pos;
	public GameObject leader;

    public override Vector3 ComputeVelocity() {
    	Vector3 baseRot = new Vector3(0.0f, 270.0f, 0.0f);
    	Vector3 rot = leader.transform.rotation.eulerAngles - baseRot;
        Quaternion qrot = Quaternion.Euler(rot.x, rot.y, rot.z);
        Vector3 aim = (qrot * pos); // rotate it
    	Vector3 v = (leader.transform.position + aim - this.transform.position)* 5.0f;
        return v;
    }

}