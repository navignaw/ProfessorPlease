using UnityEngine;
using System.Collections;
/**
 * Resets scene on game over.
 */
public class GameOver : MonoBehaviour {
    public static int GameOverCount;
    public int gameOverCount = 5;

    private static ScreenFade fader;

    // Use this for initialization
    void Start() {
        GameOverCount = gameOverCount;
        fader = GetComponent<ScreenFade>();
    }

    // Update is called once per frame
    void Update () {
    }

    public static void Lose() {
        fader.EndScene(Application.loadedLevel);
    }

}