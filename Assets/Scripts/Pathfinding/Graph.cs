using UnityEngine;
using System.Collections;

/**
 * Graph class.
 */
public abstract class Graph {
    public float dist;
    public Vector3 startpos;
    public Vector3 endpos;
    // neighbors [row (x indexed)][col (z indexed)][index into neighbor list]
    public Vector2[][][] neighbors;

    Vector2 NearestNode(Vector3 pos) {
    	return Vector2.zero;
    }
}