using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private bool sceneEnding = false;       // Whether or not the scene is still fading in.
    private int nextLevel = 0;
    private GUITexture texture;

    void Awake ()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        texture = GetComponent<GUITexture>();
        texture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    void Update ()
    {
        // If the scene is starting...
        if (sceneStarting) {
            StartScene();
        } else if (sceneEnding) {
            EndScene();
        }
    }

    void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        texture.color = Color.Lerp(texture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        texture.color = Color.Lerp(texture.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (texture.color.a <= 0.05f) {
            // ... set the colour to clear and disable the GUITexture.
            texture.color = Color.clear;
            texture.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }

    public void EndScene(int level)
    {
        nextLevel = level;
        sceneEnding = true;
    }

    public void EndScene() {
        // Make sure the texture is enabled.
        texture.enabled = true;

        // Start fading towards black.
        FadeToBlack();

        // If the screen is almost black...
        if (texture.color.a >= 0.95f) {
            sceneEnding = false;
            // ... reload the level.
            Application.LoadLevel(nextLevel);
        }
    }
}