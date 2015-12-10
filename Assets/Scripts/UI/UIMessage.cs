using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMessage : MonoBehaviour {
    public static GameObject messagePrefab;
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

    public static void SetMessagePrefab(GameObject prefab) {
        messagePrefab = prefab;
    }

     // Create text message
    public static GameObject CreateMessage(string text, Vector3 offset, float duration) {
        if (messagePrefab == null) {
            Debug.Log("must set message prefab before creating message!");
            return null;
        }

        GameObject message = Instantiate(messagePrefab) as GameObject;
        message.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);
        message.transform.localPosition = offset;
        UIMessage uiMessage = message.GetComponent<UIMessage>();
        uiMessage.messageText = text;
        uiMessage.duration = duration;
        return message;
    }

    public static GameObject CreateMessage(string text) {
        Vector3 offset = new Vector3(Random.Range(-300f, 300f), Random.Range(-200f, 200f), 0f); // generate random offset
        float duration = Random.Range(3f, 5f); // random duration between 3-5s
        return CreateMessage(text, offset, duration);
    }

    void CloseMessage() {
        Destroy(gameObject);
        Question.numMessages--;
    }
}
