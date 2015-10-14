using UnityEngine;
using System.Collections;

/**
 * Bird's eye view player follow camera script.
 */
public class CameraFollow : MonoBehaviour {
    public Transform player;
    public Vector2 limitX = new Vector2(-30f, 30f);
    public Vector2 limitZ = new Vector2(-30f, 30f);

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(player.position.x, limitX.x, limitX.y);
        newPosition.z = Mathf.Clamp(player.position.z, limitZ.x, limitZ.y);
        transform.position = newPosition;
    }

}
