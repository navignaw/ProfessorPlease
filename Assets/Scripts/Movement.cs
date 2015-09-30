using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;

/**
 * Master class that combines movement vectors from various other scripts
 */
[RequireComponent(typeof (ThirdPersonCharacter))]
public class Movement : MonoBehaviour {
    public BaseBehavior[] behaviors;

    private ThirdPersonCharacter character;

    // Use this for initialization
    void Start() {
        character = GetComponent<ThirdPersonCharacter>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 newVelocity = Vector3.zero;
        foreach (BaseBehavior behavior in behaviors) {
            if (behavior.scale == 0) {
                continue;
            }
            newVelocity += behavior.ComputeVelocity() * behavior.scale;
        }

        character.Move(newVelocity, false, false);
    }

}
