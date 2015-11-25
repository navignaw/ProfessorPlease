using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/**
 * Base student class. Inherit this when implementing anything that needs to find a target.
 */
public class BaseStudent : MonoBehaviour {
    protected GameObject target;

    public void FindProfessorTarget() {
        GameObject[] profs = GameObject.FindGameObjectsWithTag("Player");

        // Pick the closest player
        target = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject prof in profs) {
            float curDistance = (prof.transform.position - position).sqrMagnitude;
            if (curDistance < distance) {
                target = prof;
                distance = curDistance;
            }
        }
    }

    public void FindCurrentPlayer() {
        GameObject[] profs = GameObject.FindGameObjectsWithTag("Player");

        target = null;
        foreach (GameObject prof in profs) {
            NetworkIdentity nIdentity = prof.GetComponent<NetworkIdentity>();
            if (nIdentity.isLocalPlayer) {
                target = prof;
                return;
            }
        }
    }
}