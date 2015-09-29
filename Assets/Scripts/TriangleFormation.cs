using UnityEngine;
using System.Collections;

public class TriangleFormation : MonoBehaviour {
    public float separation;
    public GameObject[] formation;

    // Use this for initialization
    public void Start () {
        int i;
        int row = -1;
        int col = 1;
        for (i = 0; i < formation.Length; i++) {
            if (row > col) {
                col++;
                row = -row;
            }
            Vector3 normCoord = new Vector3(row, 0.0f, -col);
            formation[i].GetComponent<FormationFollow>().pos = 
                separation * normCoord;
            row++;
            formation[i].GetComponent<FormationFollow>().leader = this.gameObject;
        }
    }

}
