using UnityEngine;
using System.Collections;

/**
 * Asks a question (creating UIMessage) when near professor
 */
public class Question : BaseStudent {
    public static int numMessages = 0; // how many questions are active

    public GameObject messagePrefab;
    public Vector3 offset = Vector3.zero;
    public float duration = 5f;
    public float distance = 3f; // how far away before the question is asked
    public string[] text;

    private float lastAsked = 3f; // how long since last question was asked
    private int textCounter = 0;

    // Use this for initialization
    void Start() {
        Question.numMessages = 0;
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
            lastAsked = duration;
        } else if (lastAsked > 0) {
            lastAsked -= Time.deltaTime;
        }
    }

    private void AskQuestion() {
        if (text.Length > 0) {
            if (textCounter < text.Length) {
                UIMessage.CreateMessage(text[textCounter++], offset, duration);
            }
        } else {
            CreateRandomMessage();
        }

        if (++numMessages >= GameOver.GameOverCount) {
            // add some more random questions
            Invoke("CreateRandomMessage", Random.Range(0f, 2f));
            Invoke("CreateRandomMessage", Random.Range(1f, 3f));
            Invoke("CreateRandomMessage", Random.Range(2f, 4f));
            GameOver.Lose();
        }
    }

    private static string[] Questions = new string[] {
        "Professor, will splay trees be on the midterm?",
        "Professor, will infinite Ramsey theory be on the midterm?",
        "Professor, will countable sets be on the midterm?",
        "Professor, will unbounded fan-in circuits be on the midterm?",
        "Professor, will Vickrey-Clarke-Groves auctions be on the final?",
        "Professor, will gradient descent be on the final?",
        "Professor, can I get an extension on the homework, please?",
        "Professor, can I run an idea for my term project by you?",
        "Professor, how many late days do I have left for this lab?",
        "Professor, I'm so sorry but I forgot to turn in my homework yesterday! Can I please turn it in now?",
        "Professor, I was deathly ill last night with hypothermia. Can I please get an extension?",
        "Professor, I was super drunk at a party last night. Can I please get an extension?",
        "Professor, My dog ate my homework. Can I please get an extension?",
        "Professor, My pet komodo dragon ate my homework. Can I please get an extension?",
        "Professor, My pet velociraptor ate my homework. Can I please get an extension?",
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
        "Professor, what heuristic should I use for real-time adaptive A*?",
        "Professor, I'm pretty sure I got this answer on the test right. Can you take a look?",
        "Professor, I think there's a mistake with my attendance grade. Can you fix it?",
        "Professor, I think there's a mistake with my participation grade. Can you fix it?",
        "Professor, is it okay if I steal models from the internet for my game?",
        "Professor, can we get an extra cheat sheet on the exam?",
        "Professor, when and where is the review session for the final?",
        "Professor, will the class be curved?",
        "Professor, will the midterm be curved?",
        "Professor, what is your favorite game programming algorithm?"
    };

    private static string RandomQuestion {
        get {
            return Questions[Random.Range(0, Questions.Length)];
        }
    }

    private void CreateRandomMessage() {
        UIMessage.CreateMessage(Question.RandomQuestion);
    }

}
