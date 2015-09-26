using UnityEngine;
using System.Collections;

/**
 * Random wander behavior. TODO: Jun implement this
 */
public class Wander : BaseBehavior {
    public override Vector3 ComputeVelocity() {
        return this.transform.forward;
    }

}