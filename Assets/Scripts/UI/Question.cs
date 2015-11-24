using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/**
 * Asks a question (creating UIMessage) when near professor
 */
public class Question : BaseStudent {
    public GameObject messagePrefab;
    public float distance = 3f; // how far away before the question is asked

    private bool askedQuestion = false; // TODO: add countdown before they ask another question
    private int lastUpdatedTarget = 20;

    // Use this for initialization
    void Start() {
        UIMessage.SetMessagePrefab(messagePrefab);
    }

    // Update is called once per frame
    void Update () {
        if (--lastUpdatedTarget == 0) {
            FindProfessorTarget();
            lastUpdatedTarget = 20;
        }
        if (target == null) {
            return;
        }

        NetworkIdentity nIdentity = target.GetComponent<NetworkIdentity>();
        if (!askedQuestion && nIdentity.isLocalPlayer && Vector3.Distance(this.transform.position, target.transform.position) <= distance) {
            AskQuestion();
        }
    }



    private void AskQuestion() {
        UIMessage.CreateMessage(Question.RandomQuestion);
        askedQuestion = true;
    }

    private static string[] Questions = new string[] {
        "Professor, will splay trees be on the midterm?",
        "Professor, will infinite Ramsey theory be on the midterm?",
        "Professor, will countable sets be on the midterm?",
        "Professor, will unbounded fan-in circuits be on the midterm?",
        "Professor, can I get an extension on the homework, please?",
        "Professor, how many late days do I have left for this lab?",
        "Professor, I'm so sorry but I forgot to turn in my homework yesterday! Can I please turn it in now?",
        "Professor, I was deathly ill last night with hypothermia. Can I please get an extension?",
        "Professor, I was super drunk at a party last night. Can I please get an extension?",
        "Professor, are you single? My friend is really interested in taking you out to lunch...",
    };

    private static string RandomQuestion {
        get {
            return Questions[Random.Range(0, Questions.Length)];
        }
    }

}
