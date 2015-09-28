using UnityEngine;
using System.Collections;

public class SquareFormation : MonoBehaviour {
    public int size;
    public GameObject model;
    private GameObject[] formation;

    // Use this for initialization
    public void Start () {
        formation = new GameObject[size];
        int i;
        GameObject floor = GameObject.Find("Floor");
        for (i = 0; i < size; i++) {
            Vector3 pos = new Vector3 (
                Random.value * floor.transform.localScale.x,
                0.0f,
                Random.value * floor.transform.localScale.z
                );
            GameObject student = Instantiate(model, transform.position, transform.rotation) as GameObject;
            student.transform.parent = transform;
            student.transform.localPosition = pos;
            formation[i] = student;
        }
    }
    
    // Update is called once per frame
    public void Update () {
        /*int i;
        for (i = 0; i < size; i++) {
            formation[i].transform.position = 
            formation[i].transform.position - (formation[i].transform.position - transform.position) * 0.01f;
        }*/
    }

}
