using UnityEngine;
using System.Collections;

/**
 * Bird's eye view player follow camera script.
 */
public class NextLevel : MonoBehaviour {
    public int nextLevel;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Application.LoadLevel(nextLevel);
        }
    }

}
