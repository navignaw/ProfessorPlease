using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class NetworkScript : NetworkManager
{
    public GameObject button;

    public static short MSGType = 555;

    // Use this for initialization
    void Start()
    {
        button = GameObject.Find("ToggleButton");

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
        IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
    }

    private void OnClientChatMessage(NetworkMessage netMsg)
    {
        IntegerMessage msg = netMsg.ReadMessage<IntegerMessage>();
    }

    public static GameObject FindProfessorTarget() {
        GameObject[] profs = GameObject.FindGameObjectsWithTag("Player");
        if (profs.Length == 0) {
            return null;
        } else if (profs.Length == 1) {
            return profs[0];
        }

        // If more than 1 professor, then pick one at random
        return profs[Random.Range(0, profs.Length)];
    }
}
