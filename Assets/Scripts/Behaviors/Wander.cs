using UnityEngine;
using System.Collections;

/**
 * Random wander behavior. TODO: Jun implement this
 */
public class Wander : BaseBehavior {
	public float circleRadius = 10.0f;
    public float wanderStrength = 0.4f;
	public float wanderAngle = 0.0f; // change to current direction
	public float angleChange = 1f; // change in angle per frame

    private Vector3 wander() {
        // based on wander behavior proposed by Craig Reynolds

        // get circle in front of model (velocity vector)
        Vector3 circleCenter = this.transform.forward;
        circleCenter.Normalize();
        circleCenter = circleCenter*circleRadius;


        // make new force vector for random wander
        Vector3 displacement = new Vector3(0,0,0);
        displacement.x = Mathf.Cos(wanderAngle);
        displacement.z = Mathf.Sin(wanderAngle);
        displacement.Normalize();
        displacement = displacement*wanderStrength;

        wanderAngle += (Random.value-0.5f)*angleChange;

        Vector3 wanderForce = new Vector3();
        wanderForce = circleCenter+displacement;

        return wanderForce;
    }

	public override Vector3 ComputeVelocity()  {
        float speed = this.transform.forward.magnitude;

    	// apply force
        Vector3 wanderForce = wander();
        Vector3 forward = this.transform.forward;
        forward.Normalize();

        Vector3 total = forward + wanderForce;
        total.Normalize();

    	return speed*total;
    }

}