using UnityEngine;
using System.Collections;

/**
 * Base behavior class. Inherit this when implementing flocking, wander, etc.
 */
public abstract class BaseBehavior : MonoBehaviour {
    public float scale = 0;
    public abstract Vector3 ComputeVelocity();
}