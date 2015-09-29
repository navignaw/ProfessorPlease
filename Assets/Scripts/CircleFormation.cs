using UnityEngine;
using System.Collections;

public class CircleFormation : MonoBehaviour {
    public float radius;
    public GameObject[] formation;

    // Use this for initialization
    public void Start () {
        int i;
        int total = formation.Length + 1;
        for (i = 0; i < formation.Length; i++) {
            float angle = 2 * Mathf.PI * (i+1) / total;
            Vector3 normCoord = new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle) - 1.0f);
            formation[i].GetComponent<FormationFollow>().pos = 
                radius * normCoord;
            formation[i].GetComponent<FormationFollow>().leader = this.gameObject;
        }
    }

}
