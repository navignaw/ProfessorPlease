using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;

/**
 * Master class that combines movement vectors from various other scripts
 */
[RequireComponent(typeof (ThirdPersonCharacter))]
public class Movement : NetworkBehaviour {
    public BaseBehavior[] behaviors;

    [SyncVar]
    private ThirdPersonCharacter character;

    // Use this for initialization
    void Start() {
        character = GetComponent<ThirdPersonCharacter>();
    }

    // Update is called once per frame
    void Update () {
        if (!isServer) {
            return;
        }

        Vector3 normalVelocity = Vector3.zero;
        Vector3 vitalVelocity = Vector3.zero;

        foreach (BaseBehavior behavior in behaviors) {
            if (behavior.scale == 0) {
                continue;
            }
            Vector3 velocity = behavior.ComputeVelocity();

            if (behavior.vital) {
                vitalVelocity += velocity * behavior.scale;
            } else {
                normalVelocity += velocity * behavior.scale;
            }
        }

        if (vitalVelocity.magnitude < 0.1f) {
            vitalVelocity += normalVelocity;
        }
        character.Move(vitalVelocity, false, false);
    }

}
