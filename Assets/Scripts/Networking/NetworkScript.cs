using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class NetworkScript : NetworkManager
{
    public static short MSGType = 555;

    public GameObject messagePrefab;

    // Use this for initialization
    void Start()
    {
        UIMessage.SetMessagePrefab(messagePrefab);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStartClient(NetworkClient client) {
        client.RegisterHandler(MSGType, OnClientChatMessage);
    }

    // hook into NetManagers server setup process
    public override void OnStartServer() {
        base.OnStartServer();
        NetworkServer.RegisterHandler(MSGType, OnServerChatMessage);
    }

    private void OnServerChatMessage(NetworkMessage netMsg) {
        if (!NetworkManager.singleton.isNetworkActive) {
            return;
        }

        ChatMessage message = netMsg.ReadMessage<ChatMessage>();
        Debug.Log("server received " + message.text + " from client " + message.playerId.ToString());
        NetworkServer.SendToAll(MSGType, message);
    }

    private void OnClientChatMessage(NetworkMessage netMsg) {
        if (!NetworkManager.singleton.IsClientConnected()) {
            return;
        }

        ChatMessage message = netMsg.ReadMessage<ChatMessage>();
        //foreach (PlayerController player in client.connection.playerControllers) {
        //    if (player.playerControllerId != message.playerId) {
                Debug.Log("client received " + message.text + " from " + message.playerId.ToString());
                UIMessage.CreateMessage(message.text);
        //    }
        //}
    }

}
