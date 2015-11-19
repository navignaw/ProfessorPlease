using UnityEngine;
using System.Collections;

[System.Serializable]
public enum StudentState {
    Wander,
    Follow,
    Seeking
};

/**
 * Communication script for intelligence module.
 */
public class Communication : MonoBehaviour {
    public int groupId;
    public StudentState state;
    public GameObject target;
    public Vector3 targetLastKnownPos = Vector3.zero;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
        switch (state) {
            case StudentState.Wander:
                break;

            case StudentState.Follow:
                break;

            case StudentState.Seeking:
                break;
        }
    }

}
