using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMessage : MonoBehaviour {
    public Text message;
    public Image image;

    public string messageText {
        get {
            return message.text;
        }
        set {
            message.text = value;
        }
    }

    // How long message will last before fading.
    public float duration = 0;

    // Use this for initialization
    void Start () {
        Invoke("CloseMessage", duration);
    }

    // Update is called once per frame
    void Update () {
    }


    void CloseMessage() {
        Destroy(gameObject);
    }
}
