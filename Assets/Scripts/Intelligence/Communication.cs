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
public class Communication : BaseStudent {
    public int groupId = 0;
    public StudentState state;
    public Vector3 targetLastKnownPos = Vector3.zero;
    public BaseBehavior pathfind;
    public BaseBehavior wander;
    public BaseBehavior sight;
    public int followtime;
    private float wscale = 0.5f;
    private float pscale = 0.6f;
    public float stopRadius = 3f;

    private List<Communication> groupMembers;
    private int lastUpdatedTarget = 20;

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
        if (--lastUpdatedTarget == 0) {
            FindProfessorTarget();
            lastUpdatedTarget = 20;
        }
        if (target == null) {
            return;
        }

        switch (state) {
            case StudentState.Wander:
                if (sight.scale > 0.5f && target != null) {
                    state = StudentState.Follow;
                    targetLastKnownPos = target.transform.position;
                    PingGroup(target.transform.position);
                    wander.scale = 0f;
                    pathfind.scale = pscale;
                } else {
                    wander.scale = wscale;
                    pathfind.scale = 0f;
                }
                break;

            case StudentState.Follow:
                if (sight.scale > 0.5f && target != null) {
                    targetLastKnownPos = target.transform.position;
                } else {
                    state = StudentState.Seeking;
                }
                pathfind.scale = pscale;
                wander.scale = 0f;
                followtime++;
                if (followtime > 20 && target != null) {
                    PingGroup(target.transform.position);
                    followtime = 0;
                }
                break;

            case StudentState.Seeking:
                pathfind.scale = pscale;
                wander.scale = 0f;
                followtime = 0;
                if (Vector3.Distance(targetLastKnownPos, this.transform.position) < stopRadius) {
                    AntiPingGroup();
                    wander.scale = wscale;
                    pathfind.scale = 0f;
                }
                break;
        }
    }

    // message all groups with target's location
    void PingGroup(Vector3 targetPos) {
        foreach (Communication groupMember in groupMembers) {
            groupMember.targetLastKnownPos = targetPos;
            groupMember.state = StudentState.Seeking;
        }
    }

    // message all groups that target is not at location
    void AntiPingGroup() {
        foreach (Communication groupMember in groupMembers) {
            groupMember.targetLastKnownPos = Vector3.zero;
            groupMember.state = StudentState.Wander;
            groupMember.pathfind.scale = 0f;
            groupMember.wander.scale = wscale;
        }
    }

}
