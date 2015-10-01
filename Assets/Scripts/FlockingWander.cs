using UnityEngine;
using System.Collections;

public class FlockingWander : MonoBehaviour {
    public float radius;
    public float minradius;
    public GameObject[] formation;

    // Use this for initialization
    public void Start () {
        int i, j;
        int minidx;
        float mindist;
        float angle;
        float dist;
        Vector3 coord = new Vector3(0.0f, 0.0f, 0.0f);
        bool[] assigned = new bool[formation.Length];
        for (i = 0; i < formation.Length; i++) {
            assigned[i] = false;
        }
        for (i = 0; i < formation.Length; i++) {
            if (i!=0) angle = Random.value * 300f + 30f;
            else angle = Random.value * 180f + 90f;
            dist = Random.value * (radius - minradius) + minradius;
            coord.x = coord.x + dist * Mathf.Sin(angle*2.0f*Mathf.PI/360.0f);
            coord.z = coord.z + dist * Mathf.Cos(angle*2.0f*Mathf.PI/360.0f);
            minidx = -1;
            mindist = -1f;
            for (j = 0; j < formation.Length; j++) {
                Vector3 pos = formation[j].GetComponent<FormationFollow>().pos;
                if (!assigned[j] && (mindist == -1f || mindist > (coord-pos).magnitude)) {
                    mindist = (coord-pos).magnitude;
                    minidx = j;
                    assigned[j] = true;
                }
            }
            formation[minidx].GetComponent<FormationFollow>().pos = coord;
            formation[minidx].GetComponent<FormationFollow>().leader = 
                this.gameObject;
        }
    }

}