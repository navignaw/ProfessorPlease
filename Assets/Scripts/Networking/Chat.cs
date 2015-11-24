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

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("a") && isLocalPlayer) {
            ChatMessage message = new ChatMessage(playerControllerId, "test derp");
            NetworkManager.singleton.client.Send(NetworkScript.MSGType, message);
        }
    }

}
