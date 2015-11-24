using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent (typeof(NetworkAnimator))]
public class NetworkPlayerAnim : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        NetworkAnimator anim = GetComponent<NetworkAnimator>();
        anim.SetParameterAutoSend(0, true); // forward
        anim.SetParameterAutoSend(1, true); // turn
        anim.SetParameterAutoSend(3, true); // on ground
        anim.SetParameterAutoSend(4, true); // jump
    }

    // Update is called once per frame
    void Update()
    {

    }
}