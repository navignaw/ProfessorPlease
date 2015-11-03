using UnityEngine;
using System.Collections;

/**
 * Asks a question (creating UIMessage) when near professor
 */
public class Question : MonoBehaviour {
    public GameObject messagePrefab;
    public string messageText; // TODO: pull from array of random messages
    public float distance = 3f; // how far away before the question is asked

    private Transform canvas;
    private GameObject target;
    private bool askedQuestion = false;

    // Use this for initialization
    void Start() {
        target = GameObject.FindWithTag("Player");
        canvas = GameObject.FindWithTag("Canvas").transform;
    }

    // Update is called once per frame
    void Update () {
        if (!askedQuestion && Vector3.Distance(this.transform.position, target.transform.position) <= distance) {
            AskQuestion();
        }
    }

     // Create text message
    private GameObject CreateMessage(string text, Vector3 offset, float duration) {
        GameObject message = Instantiate(messagePrefab) as GameObject;
        message.transform.SetParent(canvas, false);
        message.transform.localPosition = offset;
        UIMessage uiMessage = message.GetComponent<UIMessage>();
        uiMessage.messageText = text;
        uiMessage.duration = duration;
        return message;
    }


    private void AskQuestion() {
        Vector3 offset = new Vector3((Random.value - 0.5f) * 800f, (Random.value - 0.5f) * 400f, 0f); // generate random offset
        float duration = 3f + Random.value * 2f; // random duration between 3-5s
        CreateMessage(messageText, offset, duration);
        askedQuestion = true;
    }

}
