using UnityEngine;
using System.Collections;

/**
 * Resets scene on game over.
 */
public class GameOver : MonoBehaviour {
    public static int gameOverCount = 5;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
    }

    public static void Lose() {
        // TODO: trigger gameover screen or fadeout?
        Application.LoadLevel(Application.loadedLevel);
    }

}