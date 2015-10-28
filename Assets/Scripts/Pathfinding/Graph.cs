using UnityEngine;
using System;
using System.Collections;

public struct Node3D {
    public int x, y, z;

    public Node3D(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override bool Equals(System.Object obj) {
        return obj is Node3D && this == (Node3D)obj;
    }
    public override int GetHashCode() {
        return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }
    public static bool operator ==(Node3D u, Node3D v) {
        return u.x == v.x && u.y == v.y && u.z == v.z;
    }
    public static bool operator !=(Node3D u, Node3D v) {
        return !(u == v);
    }
}

/**
 * Graph class.
 */
public class Graph : MonoBehaviour {
    public float dist;
    public float vdist;
    public float checkwidth;
    public Vector3 startpos;
    public Vector3 endpos;
    // neighbors [row (x-indexed)][height (y-indexed)][col (z-indexed)][index into neighbor list]
    public Node3D[][][][] neighbors;
    public int xgrid;
    public int ygrid;
    public int zgrid;

    private float startx;
    private float starty;
    private float startz;
    private string collisionTag = "Wall";
    private bool[][][] valid;

    private bool is_valid(int i, int j, int k) {
        return i >= 0 && j >= 0 && k >= 0 && i <= xgrid && j <= ygrid && k <= zgrid && valid[i][j][k];
    }

    // Use this for initialization
    void Start () {
        RaycastHit hit = new RaycastHit();
        float width = Mathf.Abs(endpos.x - startpos.x);
        float depth = Mathf.Abs(endpos.y - startpos.y);
        float height = Mathf.Abs(endpos.z - startpos.z);
        xgrid = Mathf.CeilToInt(width / dist);
        ygrid = Mathf.CeilToInt(depth / vdist);
        zgrid = Mathf.CeilToInt(height / dist);
        startx = Mathf.Min(startpos.x, endpos.x);
        starty = Mathf.Min(startpos.y, endpos.y);
        startz = Mathf.Min(startpos.z, endpos.z);
        int i, j, k;

        // Compute valid vertices (not floating in space)
        valid = new bool[xgrid + 1][][];
        for (i = 0; i <= xgrid; i++) {
            valid[i] = new bool[ygrid + 1][];
            for (j = 0; j <= ygrid; j++) {
                valid[i][j] = new bool[zgrid + 1];
                for (k = 0; k <= zgrid; k++) {
                    Vector3 currpos = new Vector3(startx + i*dist, starty + j*vdist, startz + k*dist);
                    if (Physics.Raycast(currpos, Vector3.down, out hit, vdist) && hit.collider.tag == collisionTag) {
                        float toGround = hit.distance;
                        bool rightleft = Physics.Raycast(currpos + checkwidth * Vector3.right, Vector3.down, out hit, toGround + 0.001f) && hit.collider.tag == collisionTag
                            && Physics.Raycast(currpos + checkwidth * Vector3.left, Vector3.down, out hit, toGround + 0.001f) && hit.collider.tag == collisionTag;
                        bool forwardback = Physics.Raycast(currpos + checkwidth * Vector3.forward, Vector3.down, out hit, toGround + 0.001f) && hit.collider.tag == collisionTag
                            && Physics.Raycast(currpos + checkwidth * Vector3.back, Vector3.down, out hit, toGround + 0.001f) && hit.collider.tag == collisionTag;
                        if (rightleft && forwardback) {
                            valid[i][j][k] = true;
                        } else if (rightleft && Physics.Raycast(currpos + checkwidth * Vector3.forward, Vector3.down, out hit, toGround + checkwidth/2f + 0.001f) && hit.collider.tag == collisionTag
                            && Physics.Raycast(currpos + checkwidth * Vector3.back, Vector3.down, out hit, toGround + checkwidth/2f + 0.001f) && hit.collider.tag == collisionTag) {
                            valid[i][j][k] = true;
                        } else if (forwardback && Physics.Raycast(currpos + checkwidth * Vector3.right, Vector3.down, out hit, toGround + checkwidth/2f + 0.001f) && hit.collider.tag == collisionTag
                            && Physics.Raycast(currpos + checkwidth * Vector3.left, Vector3.down, out hit, toGround + checkwidth/2f + 0.001f) && hit.collider.tag == collisionTag) {
                            valid[i][j][k] = true;
                        }
                    } else {
                        valid[i][j][k] = false;
                    }
                }
            }
        }

        // Compute neighbors and edges
        neighbors = new Node3D[xgrid + 1][][][];
        for (i = 0; i <= xgrid; i++) {
            neighbors[i] = new Node3D[ygrid + 1][][];
            for (j = 0; j <= ygrid; j++) {
                neighbors[i][j] = new Node3D[zgrid + 1][];
                Node3D[] temp = new Node3D[12];
                float ramplen = Mathf.Sqrt(Mathf.Pow(dist, 2f) + Mathf.Pow(vdist, 2f));
                for (k = 0; k <= zgrid; k++) {
                    if (!is_valid(i, j, k)) {
                        neighbors[i][j][k] = new Node3D[0];
                        continue;
                    }
                    int count = 0;
                    Vector3 currpos = new Vector3(startx + i*dist, starty + j*vdist, startz + k*dist);
                    if (is_valid(i, j, k+1) && !(Physics.Raycast(currpos, Vector3.forward, out hit, dist) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i, j, k+1);
                        count++;
                    }
                    if (is_valid(i, j, k-1) && !(Physics.Raycast(currpos, Vector3.back, out hit, dist) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i, j, k-1);
                        count++;
                    }
                    if (is_valid(i-1, j, k) && !(Physics.Raycast(currpos, Vector3.left, out hit, dist) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i-1, j, k);
                        count++;
                    }
                    if (is_valid(i+1, j, k) && !(Physics.Raycast(currpos, Vector3.right, out hit, dist) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i+1, j, k);
                        count++;
                    }
                    if (is_valid(i, j+1, k+1) && !(Physics.Raycast(currpos, dist * Vector3.forward + vdist * Vector3.up, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i, j+1, k+1);
                        count++;
                    }
                    if (is_valid(i, j+1, k-1) && !(Physics.Raycast(currpos, dist * Vector3.back + vdist * Vector3.up, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i, j+1, k-1);
                        count++;
                    }
                    if (is_valid(i+1, j+1, k) && !(Physics.Raycast(currpos, dist * Vector3.right + vdist * Vector3.up, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i+1, j+1, k);
                        count++;
                    }
                    if (is_valid(i-1, j+1, k) && !(Physics.Raycast(currpos, dist * Vector3.left + vdist * Vector3.up, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i-1, j+1, k);
                        count++;
                    }
                    if (is_valid(i, j-1, k+1) && !(Physics.Raycast(currpos, dist * Vector3.forward + vdist * Vector3.down, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i, j-1, k+1);
                        count++;
                    }
                    if (is_valid(i, j-1, k-1) && !(Physics.Raycast(currpos, dist * Vector3.back + vdist * Vector3.down, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i, j-1, k-1);
                        count++;
                    }
                    if (is_valid(i+1, j-1, k) && !(Physics.Raycast(currpos, dist * Vector3.right + vdist * Vector3.down, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i+1, j-1, k);
                        count++;
                    }
                    if (is_valid(i-1, j-1, k) && !(Physics.Raycast(currpos, dist * Vector3.left + vdist * Vector3.down, out hit, ramplen) && hit.collider.tag == collisionTag)) {
                        temp[count] = new Node3D(i-1, j-1, k);
                        count++;
                    }
                    int l;
                    neighbors[i][j][k] = new Node3D[count];
                    for (l = 0; l < count; l++) {
                        neighbors[i][j][k][l] = new Node3D(temp[l].x, temp[l].y, temp[l].z);
                    }
                }

            }
        }
    }

    public Node3D NearestNode(Vector3 pos) {
        float posx = pos.x - startx;
        float posy = pos.y - starty;
        float posz = pos.z - startz;
        int retx, rety, retz;
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
        if (posy < 0f) {
            rety = 0;
        } else if (posy > vdist * (ygrid - 1)) {
            rety = ygrid - 1;
        } else {
            int lowery = Mathf.FloorToInt(posy / vdist);
            if (Mathf.Abs(posy - lowery * vdist) < Mathf.Abs((lowery + 1) * vdist - posy)) {
                rety = lowery;
            } else {
                rety = lowery + 1;
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

        return new Node3D(retx, rety, retz);
    }

    public Vector3 WorldPosition(Node3D pos) {
        Vector3 result = new Vector3(startx + pos.x * dist, starty + pos.y * vdist, startz + pos.z * dist);
        return result;
    }

    void OnDrawGizmos() {
        for (int i = 0; i < xgrid; i++) {
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < zgrid; k++) {
                    if (valid[i][j][k]) {
                        Gizmos.color = Color.yellow;
                    } else {
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawSphere(new Vector3(startx + i * dist, starty + j * vdist, startz + k * dist), 0.2f);

                    /*for (int l = 0; l < neighbors[i][j][k].Length; l++) {
                        Gizmos.DrawLine(WorldPosition(new Node3D(i,j,k)), WorldPosition(neighbors[i][j][k][l]));
                    }*/
                }
            }
        }


    }
}