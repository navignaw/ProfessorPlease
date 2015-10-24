using UnityEngine;
using System;
using System.Collections;

public struct Node2D {
    public int x, z;

    public Node2D(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(System.Object obj) {
        return obj is Node2D && this == (Node2D)obj;
    }
    public override int GetHashCode() {
        return x.GetHashCode() ^ z.GetHashCode();
    }
    public static bool operator ==(Node2D u, Node2D v) {
        return u.x == v.x && u.z == v.z;
    }
    public static bool operator !=(Node2D u, Node2D v) {
        return !(u == v);
    }
}

/**
 * Graph class.
 */
public class Graph : MonoBehaviour {
    public float dist;
    public Vector3 startpos;
    public Vector3 endpos;
    // neighbors [row (x-indexed)][col (z-indexed)][index into neighbor list]
    public Node2D[][][] neighbors;
    public int xgrid;
    public int zgrid;
    public float yheight;

    private float startx;
    private float startz;
    private string collisionTag = "Wall";

    // Use this for initialization
    void Start () {
        RaycastHit hit = new RaycastHit();
        float width = Mathf.Abs(endpos.x - startpos.x);
        float height = Mathf.Abs(endpos.z - startpos.z);
        xgrid = Mathf.CeilToInt(width / dist);
        zgrid = Mathf.CeilToInt(height / dist);
        int i, j;
        neighbors = new Node2D[xgrid + 1][][];
        startx = Mathf.Min(startpos.x, endpos.x);
        startz = Mathf.Min(startpos.z, endpos.z);
        for (i = 0; i <= xgrid; i++) {
            neighbors[i] = new Node2D[zgrid + 1][];
            for (j = 0; j <= zgrid; j++) {
                int count = 0;
                Node2D[] temp = new Node2D[4];
                Vector3 currpos = new Vector3(startx + i*dist, yheight, startz + j*dist);
                if (j <= zgrid && !(Physics.Raycast(currpos, Vector3.forward, out hit, dist * 1.0f) && hit.collider.tag == collisionTag)) {
                    temp[count] = new Node2D(i, j+1);
                    count++;
                }
                if (j > 0 && !(Physics.Raycast(currpos, Vector3.back, out hit, dist * 1.0f) && hit.collider.tag == collisionTag)) {
                    temp[count] = new Node2D(i, j-1);
                    count++;
                }
                if (i > 0 && !(Physics.Raycast(currpos, Vector3.left, out hit, dist * 1.0f) && hit.collider.tag == collisionTag)) {
                    temp[count] = new Node2D(i-1, j);
                    count++;
                }
                if (i <= xgrid && !(Physics.Raycast(currpos, Vector3.right, out hit, dist * 1.0f) && hit.collider.tag == collisionTag)) {
                    temp[count] = new Node2D(i+1, j);
                    count++;
                }
                int k;
                neighbors[i][j] = new Node2D[count];
                for (k = 0; k < count; k++) {
                    neighbors[i][j][k] = new Node2D(temp[k].x, temp[k].z);
                }

            }
        }
    }

    public Node2D NearestNode(Vector3 pos) {
        float posx = pos.x - startx;
        float posz = pos.z - startz;
        int retx;
        int retz;
        if (posx < 0f) {
            retx = 0;
        } else if (posx > dist * (xgrid - 1)) {
            retx = xgrid - 1;
        } else {
            int lowerx = Mathf.FloorToInt(posx / dist);
            if (Mathf.Abs(posx - lowerx * dist) < Mathf.Abs((lowerx + 1) * dist - posx)) {
                retx = lowerx;
            } else {
                retx = lowerx + 1;
            }
        }
        if (posz < 0f) {
            retz = 0;
        } else if (posz > dist * (zgrid - 1)) {
            retz = zgrid - 1;
        } else {
            int lowerz = Mathf.FloorToInt(posz / dist);
            if (Mathf.Abs(posz - lowerz * dist) < Mathf.Abs((lowerz + 1) * dist - posz)) {
                retz = lowerz;
            } else {
                retz = lowerz + 1;
            }
        }

        return new Node2D(retx, retz);
    }

    public Vector3 WorldPosition(Node2D pos, float y) {
        Vector3 result = new Vector3(startx + pos.x * dist, y, startz + pos.z * dist);
        return result;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < xgrid; i++) {
            for (int j = 0; j < zgrid; j++) {
                Gizmos.DrawSphere(WorldPosition(new Node2D(i, j), 2f), 0.2f);
            }
        }


    }
}