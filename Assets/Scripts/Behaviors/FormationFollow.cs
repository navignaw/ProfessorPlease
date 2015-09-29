﻿using UnityEngine;
using System.Collections;

/**
 * Follow the leader in formation.
 */
public class FormationFollow : BaseBehavior {
	public Vector3 pos;
	public GameObject leader;

    public override Vector3 ComputeVelocity() {
    	Vector3 rot = leader.transform.rotation.eulerAngles;
        Quaternion qrot = Quaternion.Euler(rot.x, rot.y, rot.z);
        Vector3 aim = qrot * pos;
    	Vector3 v = (leader.transform.position + aim - this.transform.position) * 5.0f;
        return v;
    }

}