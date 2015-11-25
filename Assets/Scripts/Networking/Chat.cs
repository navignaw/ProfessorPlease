using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

/**
 * Sends a message (creating UIMessage) to another player character
 */

public class ChatMessage : MessageBase
{
    public short playerId;
    public string text;

    public ChatMessage() {}

    public ChatMessage(short playerId, string text) {
        this.playerId = playerId;
        this.text = text;
    }
}

public class Chat : NetworkBehaviour {
    private string currentMessage = "";

    // Update is called once per frame
    void Update () {
    }

    void OnGUI() {
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        currentMessage = GUILayout.TextField(currentMessage);
        if (Event.current.keyCode == KeyCode.Return) {
            // Send message if not blank
            if (!string.IsNullOrEmpty(currentMessage.Trim())) {
                ChatMessage message = new ChatMessage(playerControllerId, currentMessage);
                NetworkManager.singleton.client.Send(NetworkScript.MSGType, message);
                currentMessage = "";
                GUI.FocusControl("");
            }
        }
        GUILayout.EndHorizontal();
    }

}
