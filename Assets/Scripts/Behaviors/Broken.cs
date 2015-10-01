using UnityEngine;
using System.Collections;

/**
 * Random wander behavior. TODO: Jun implement this
 */
public class Wander : BaseBehavior {
    public float circleRadius = 6f;
    public float wanderStrength = 10f;
    public float wanderAngle = 0.0f; // change to current direction
    public float angleChange = 1f; // change in angle per frame

    public float collisionStrength = 3f;
    public float collisionDangerRadius = 10f;

    private Vector3 wander() {
        float newAngleChange = (Random.value-0.5f)*angleChange;

        wanderAngle += newAngleChange;
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
        //displacement = displacement;

        

        Vector3 wanderForce = new Vector3();


        Vector3 forward = this.transform.forward;

        RaycastHit hit = new RaycastHit();

        //wanderAngle += newAngleChange;
        wanderForce = circleCenter+displacement;

        wanderForce.Normalize();

        return wanderForce;
    }


    public Vector3 collision() {
        //object[] obj = GameObject.FindGameObjectsWithTag("Wall");

        Vector3 avoidForce = new Vector3(0,0,0); //this.transform.forward;

        Vector3 forward = this.transform.forward;

        RaycastHit hit = new RaycastHit();

        float maxDist = collisionDangerRadius;

        if (Physics.Raycast(this.transform.position,forward,out hit,15)) {
            if (hit.collider.tag=="Wall") {
                Vector3 normal = hit.normal;
                normal.Normalize();
                Vector3 parallel = forward - normal*Vector3.Dot(normal,forward);
                parallel.Normalize();
                float dist = hit.distance;
                avoidForce = Vector3.Reflect(forward,normal); //(parallel)+(4f)*normal;


            }
        }

        avoidForce.Normalize();
        //print avoidForce;

        print (avoidForce);
        return avoidForce; //collisionStrength*avoidForce;
    }


    public override Vector3 ComputeVelocity() {
        float speed = this.transform.forward.magnitude;

        // apply force
        Vector3 wanderForce = wander();
        Vector3 collisionForce = collision();

        Vector3 forward = this.transform.forward;
        forward.Normalize();

        Vector3 total = forward + wanderForce;
        total.Normalize();

        Vector3 new_forward = wanderForce;

        new_forward.Normalize();

        if (collisionForce.magnitude>0.1) {
            return collisionForce;
        }

        return speed*new_forward;
    }

}