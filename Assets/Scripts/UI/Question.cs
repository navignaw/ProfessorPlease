using UnityEngine;
using System.Collections;

/**
 * Asks a question (creating UIMessage) when near professor
 */
public class Question : BaseStudent {
    public static int numMessages = 0; // how many questions are active

    public GameOver gameOver;
    public GameObject messagePrefab;
    public float distance = 3f; // how far away before the question is asked
    public string[] text;

    private float lastAsked = 3f; // how long since last question was asked
    private int textCounter = 0;

    // Use this for initialization
    void Start() {
        UIMessage.SetMessagePrefab(messagePrefab);
        FindProfessorTarget();
    }

    // Update is called once per frame
    void Update () {
        if (target == null) {
            return;
        }

        if (lastAsked <= 0 && Vector3.Distance(this.transform.position, target.transform.position) <= distance) {
            AskQuestion();
            lastAsked = 5f;
        } else if (lastAsked > 0) {
            lastAsked -= Time.deltaTime;
        }
    }

    private void AskQuestion() {
        if (text.Length > 0) {
            if (textCounter < text.Length) {
                UIMessage.CreateMessage(text[textCounter++]);
            }
        } else {
            UIMessage.CreateMessage(Question.RandomQuestion);
        }

        if (++numMessages >= GameOver.GameOverCount) {
            numMessages = 0;
            GameOver.Lose();
        }
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
        "Professor, can you explain the Naive Bayes classifier to me?",
        "Professor, can you explain bounding volume hierarchies to me?",
        "Professor, can you explain finite state automata to me?",
        "Professor, can you explain alpha-beta pruning to me?",
        "Professor, is there anything I can do to pass this class?",
        "Professor, I don't understand local perception filters.",
        "Professor, I don't understand broadband collision detection!",
        "Professor, I don't get uniform spatial subdivision!",
        "Professor, I don't get skeletonization or Voronoi diagrams!",
        "Professor, I'm pretty sure I got this answer on the test right.",
        "Professor, I think there's a mistake with my attendance grade.",
        "Professor, I think there's a mistake with my participation grade.",
        "Professor, will the class be curved?",
        "Professor, will the midterm be curved?",
        "Professor, what is your favorite game programming algorithm?"
    };

    private static string RandomQuestion {
        get {
            return Questions[Random.Range(0, Questions.Length)];
        }
    }

}
