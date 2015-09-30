using UnityEngine;
using System.Collections;

public class FlockingWander : MonoBehaviour {
    public float radius;
    public float innerradius;
    public GameObject[] formation;

    // Use this for initialization
    public void Start () {
        int i;
        float angle;
        float dist;
        Vector3 coord = new Vector3(0.0f, 0.0f, 0.0f);
        for (i = 0; i < formation.Length; i++) {
            angle = Random.value * 270.0f + 45.0f;
            dist = Random.value * (radius - innerradius) + innerradius;
            coord.x = Mathf.Sin(angle*2.0f*Mathf.PI/360.0f);
            coord.z = Mathf.Cos(angle*2.0f*Mathf.PI/360.0f)-1.0f;
            formation[i].GetComponent<FormationFollow>().pos = 
                dist * coord;
            formation[i].GetComponent<FormationFollow>().leader = 
                this.gameObject;
        }
    }
}