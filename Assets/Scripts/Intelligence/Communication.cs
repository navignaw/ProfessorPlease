using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public int groupId = 0;
    public StudentState state;
    public GameObject target;
    public Vector3 targetLastKnownPos = Vector3.zero;

    private List<Communication> groupMembers;

    // Use this for initialization
    void Start() {
        // Iterate through all students, add students with same groupId to groupMembers
        groupMembers = new List<Communication>();
        Communication[] allStudents = FindObjectsOfType(typeof(Communication)) as Communication[];
        for (int i = 0; i < allStudents.Length; i++) {
            if (allStudents[i] != this && allStudents[i].groupId == groupId) {
                groupMembers.Add(allStudents[i]);
            }
        }
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

    // message all groups with target's location
    void PingGroup(Vector3 targetPos) {
        foreach (Communication groupMember in groupMembers) {
            if (groupMember.state != StudentState.Follow) {
                groupMember.targetLastKnownPos = targetPos;
                groupMember.state = StudentState.Seeking;
            }
        }
    }

}
