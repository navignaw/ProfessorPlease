using UnityEngine;
using System.Collections;

/**
 * Master class that combines movement vectors from various other scripts
 */
public class Movement : MonoBehaviour {
    public BaseBehavior[] behaviors;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
        Vector3 newVelocity = Vector3.zero;
        foreach (BaseBehavior behavior in behaviors) {
            newVelocity += behavior.ComputeVelocity() * behavior.scale;
        }

        // TODO: set rigidbody acceleration or velocity?
    }

}
