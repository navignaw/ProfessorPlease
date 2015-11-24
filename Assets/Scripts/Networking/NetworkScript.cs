using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class NetworkScript : NetworkManager
{
    public static short MSGType = 555;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStartClient(NetworkClient mClient)
    {
        mClient.RegisterHandler(MSGType, OnClientChatMessage);
    }



    // hook into NetManagers server setup process
    public override void OnStartServer()
    {
        base.OnStartServer(); //base is empty
        NetworkServer.RegisterHandler(MSGType, OnServerChatMessage);
    }

    private void OnServerChatMessage(NetworkMessage netMsg)
    {
        //IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
    }

    private void OnClientChatMessage(NetworkMessage netMsg)
    {
        //IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
    }
}
