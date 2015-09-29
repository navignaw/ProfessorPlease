using UnityEngine;
using System.Collections;

/**
 * Random wander behavior. TODO: Jun implement this
 */
public class Wander : BaseBehavior {
	public float circleRadius = 0.5f;
    public float wanderStrength = 1.2f;
	public float wanderAngle = 0.0f; // change to current direction
	public float angleChange = 0.1f; // change in angle per frame

	public override Vector3 ComputeVelocity()  {
        print(wanderAngle);
		// based on wander behavior proposed by Craig Reynolds
        float speed = this.transform.forward.magnitude;
    	
    	// get circle in front of model (velocity vector)
    	Vector3 circleCenter = this.transform.forward;
    	circleCenter.Normalize();
    	circleCenter = circleCenter*circleRadius;


    	// make new force vector for random wander
    	Vector3 displacement = new Vector3(0,0,0);
    	displacement.x = Mathf.Cos(wanderAngle)*wanderStrength;
    	displacement.z = Mathf.Sin(wanderAngle)*wanderStrength;

    	wanderAngle += (Random.value-0.5f)*angleChange;

    	// apply force
    	Vector3 wanderForce = new Vector3();
    	wanderForce = circleCenter+displacement;

        Vector3 total = 0.7f*this.transform.forward + 0.3f*wanderForce;
        total.Normalize();

    	return speed*total;
    }

}