using UnityEngine;
using System.Collections;

public class SquareFormation : MonoBehaviour {
    public float separation;
    public GameObject model;
    public GameObject[] formation;


    // Use this for initialization
    public void Start () {
        int i;
        // assumes size of array is a square
        int side = Mathf.FloorToInt(Mathf.Sqrt(formation.Length + 1));
        int leaderside = ((side - 1) / 2);
        for (i = 0; i < formation.Length; i++) {
            if (i < leaderside) {
                formation[i].GetComponent<SquareFormationFollow>().pos = 
                    new Vector3(-separation*(i+1), 0.0f, 0.0f);
                Debug.Log(formation[i].GetComponent<SquareFormationFollow>().pos);
            } else if (i < side - 1) {
                formation[i].GetComponent<SquareFormationFollow>().pos = 
                    new Vector3(separation*(i-leaderside+1), 0.0f, 0.0f);
            }
            else {
                int row = ((i - side + 1) % side) - leaderside;
                int col = Mathf.FloorToInt((i + 1) / side);
                formation[i].GetComponent<SquareFormationFollow>().pos = 
                    new Vector3(separation*row, 0.0f, -separation*col);
            }
        }
    }
    
    // Update is called once per frame
    public void Update () {
        //nothing to update?
    }

}
