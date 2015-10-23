using UnityEngine;
using System.Collections;

/**
 * Graph class.
 */
public class Graph : MonoBehaviour {
    public float dist;
    public Vector3 startpos;
    public Vector3 endpos;
    // neighbors [row (x indexed)][col (z indexed)][index into neighbor list]
    public Vector2[][][] neighbors;
    public int xgrid;
    public int zgrid;
    public float yheight;
    private float startx;
    private float startz;

    // Use this for initialization
    void Start () {
        RaycastHit hit = new RaycastHit();
        float width = Mathf.Abs(endpos.x - startpos.x);
        float height = Mathf.Abs(endpos.z - startpos.z);
        xgrid = Mathf.CeilToInt(width / dist);
        zgrid = Mathf.CeilToInt(height / dist);
        int i, j;
        Vector3 up = new Vector3(0.0f,0.0f,1.0f);
        Vector3 down = new Vector3(0.0f,0.0f,-1.0f);
        Vector3 left = new Vector3(-1.0f,0.0f,0.0f);
        Vector3 right = new Vector3(1.0f,0.0f,0.0f);
        neighbors = new Vector2[xgrid][][];
        startx = Mathf.Min(startpos.x, endpos.x);
        startz = Mathf.Min(startpos.z, endpos.z);
        for (i = 0; i < xgrid; i++) {
            neighbors[i] = new Vector2[zgrid][];
            for (j = 0; j < zgrid; j++) {
                //Debug.Log(new Vector2(i,j));
                int count = 0;
                Vector2[] temp = new Vector2[4];
                Vector3 currpos = new Vector3(startx + i*dist, yheight, startz + j*dist);
                if (j < zgrid - 1 && Physics.Raycast(currpos, up, out hit, dist * 1.5f)) {
                    temp[count] = new Vector2(i,j+1);
                    count++;
                }
                if (j > 0 && Physics.Raycast(currpos, down, out hit, dist * 1.5f) && hit.collider.tag == "Wall") {
                    temp[count] = new Vector2(i,j-1);
                    count++;
                }
                if (i > 0 && Physics.Raycast(currpos, left, out hit, dist * 1.5f) && hit.collider.tag == "Wall") {
                    temp[count] = new Vector2(i-1,j);
                    count++;
                }
                if (i < xgrid - 1 && Physics.Raycast(currpos, right, out hit, dist * 1.5f) && hit.collider.tag == "Wall") {
                    temp[count] = new Vector2(i+1,j);
                    count++;
                }
                int k;
                neighbors[i][j] = new Vector2[count];
                //Debug.Log(count);
                for (k = 0; k < count; k++) {
                    neighbors[i][j][k] = temp[k];
                    //Debug.Log(WorldPosition(temp[k], 0.0f));
                }

            }
        }
    }

    public Vector2 NearestNode(Vector3 pos) {
        float posx = pos.x - startx;
        float posz = pos.z - startz;
        int retx;
        int retz;
        if (posx < 0f) {
            retx = 0;
        } else if (posx > dist * (xgrid - 1)) {
            retx = xgrid - 1;
        } else {
            int lowerx = Mathf.FloorToInt((posx - startx) / dist);
            if (Mathf.Abs(posx - lowerx * dist) > Mathf.Abs((lowerx + 1) * dist - posx)) {
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
            int lowerz = Mathf.FloorToInt((posz - startz) / dist);
            if (Mathf.Abs(posz - lowerz * dist) > Mathf.Abs((lowerz + 1) * dist - posz)) {
                retz = lowerz;
            } else {
                retz = lowerz + 1;
            }
        }

        return new Vector2(retx, retz);

    public Vector3 WorldPosition(Vector2 pos, float y) {
        Vector3 result = new Vector3(startx + pos.x * dist, y, startz + pos.y * dist);
        return result;
    }

    void OnDrawGizmos() {
        int i, j;
        Gizmos.color = Color.yellow;
        for (i = 0; i < xgrid; i++) {
            for (j = 0; j < zgrid; j++) {
                    Gizmos.DrawSphere(WorldPosition(new Vector2(i,j), 2f), 0.2f);
            }
        }
    }
}