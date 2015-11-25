using UnityEngine;
using System.Collections;

/**
 * Bird's eye view player follow camera script.
 */
public class CameraFollow : BaseStudent {
    public Transform player;
    public Vector2 limitX = new Vector2(-30f, 30f);
    public Vector2 limitZ = new Vector2(-30f, 30f);

    // Use this for initialization
    void Start() {
        FindCurrentPlayer();
        if (target != null) {
            player = target.transform;
        }
    }

    // Update is called once per frame
    void Update () {
        if (player == null) {
            FindCurrentPlayer();
            if (target == null) {
                return;
            }
            player = target.transform;
        }

        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(player.position.x, limitX.x, limitX.y);
        newPosition.z = Mathf.Clamp(player.position.z, limitZ.x, limitZ.y);
        transform.position = newPosition;
    }

}
