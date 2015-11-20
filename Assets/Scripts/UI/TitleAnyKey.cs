using UnityEngine;
using UnityEngine.UI;

public class TitleAnyKey : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (Input.anyKey) {
            Application.LoadLevel(1);
        }
    }

}
