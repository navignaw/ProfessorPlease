using UnityEngine;
using System.Collections;

public class VShapeFormation : MonoBehaviour {
    public float separation;
    public float angle;
    public GameObject[] formation;

    // Use this for initialization
    public void Start () {
        int i;
        Vector3 dir = Vector3.zero;
        for (i = 0; i < formation.Length; i++) {
            dir.x = 0.0f;
            dir.z = -((i - (i % 2)) / 2) - 1;
            if (i % 2 == 0) {
                dir = Quaternion.Euler(0.0f, angle, 0.0f) * dir;
            } else {
                dir = Quaternion.Euler(0.0f, -angle, 0.0f) * dir;
            }
            formation[i].GetComponent<FormationFollow>().pos = 
                separation * dir;
            formation[i].GetComponent<FormationFollow>().leader = this.gameObject;
        }
    }

}
