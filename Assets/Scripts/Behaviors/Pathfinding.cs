using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PriorityQueue;

/**
 * Move towards a target goal position via A* pathfinding magic.
 */
public class Pathfinding : BaseBehavior {
    public float stopRadius = 3f;
    public float waitTime = 1f; // how long to wait between each A* call
    public Vector3 targetv;
	public GameObject target;
    public Graph graph;

    private List<Vector3> path;
    private float waitTimer = 0f;

    private void Awake() {
        path = new List<Vector3>();
    }

    // Heuristic function for A*. Use Euclidean distance for now
    private float HeuristicValue(Node3D node, Node3D targetNode) {
        return Vector3.Distance(graph.WorldPosition(node), graph.WorldPosition(targetNode));
    }

    // Run A* and update the path of nodes we want to travel
    private void ComputePath(Vector3 startPos, Vector3 targetPos) {
        Node3D startNode = graph.NearestNode(startPos);
        Node3D targetNode = graph.NearestNode(targetPos);

        HashSet<Node3D> visited = new HashSet<Node3D>();
        PriorityQueue<float, Node3D> frontier = new PriorityQueue<float, Node3D>();
        frontier.Enqueue(0f, startNode);

        // initialize map of parents (in-edge neighbor to each vertex) for path reconstruction
        Node3D[,,] parents = new Node3D[graph.xgrid + 1, graph.ygrid + 1, graph.zgrid + 1];
        parents[startNode.x, startNode.y, startNode.z] = startNode;

        // initialize costs
        float[,,] costs = new float[graph.xgrid + 1, graph.ygrid + 1, graph.zgrid + 1];
        for (int i = 0; i < graph.xgrid; i++) {
            for (int j = 0; j < graph.ygrid; j++) {
                for (int k = 0; k < graph.zgrid; k++) {
                    if (i == startNode.x && j == startNode.y && k == startNode.z) {
                        costs[i, j, k] = 0f;
                    } else {
                        costs[i, j, k] = -1f;
                    }
                }
            }
        }

        bool foundPath = false;
        Node3D current;
        while (!frontier.IsEmpty) {
            current = frontier.DequeueValue();
            if (current == targetNode) {
                foundPath = true;
                break;
            }
            visited.Add(current);

            Node3D[] neighbors = graph.neighbors[current.x][current.y][current.z];
            for (int i = 0; i < neighbors.Length; i++) {
                if (visited.Contains(neighbors[i])) {
                    continue;
                }

                float cost = costs[current.x, current.y, current.z] + graph.dist;
                // If we found a better cost/distance, add to frontier
                if (costs[neighbors[i].x, neighbors[i].y, neighbors[i].z] < 0f || cost < costs[neighbors[i].x, neighbors[i].y, neighbors[i].z]) {
                    costs[neighbors[i].x, neighbors[i].y, neighbors[i].z] = cost;
                    parents[neighbors[i].x, neighbors[i].y, neighbors[i].z] = current;
                    float heuristic = HeuristicValue(neighbors[i], targetNode);
                    frontier.Enqueue(cost + heuristic, neighbors[i]);
                }
            }
        }

        // Either we found a path, or finished searching with no results
        if (!foundPath) {
            return;
        }

        // Reconstruct path using parents
        current = targetNode;
        List<Vector3> newPath = new List<Vector3>();
        newPath.Add(graph.WorldPosition(current));
        while (current != startNode) {
            current = parents[current.x, current.y, current.z];
            newPath.Add(graph.WorldPosition(current));
        }

        // Smooth path by deleting unneeded nodes (going in reverse)
        path.Clear();
        int p0 = newPath.Count - 1;
        int p1 = newPath.Count - 2;
        path.Add(newPath[p0]);

        RaycastHit hit = new RaycastHit();
        while (p1 >= 0) {
            float dist = Vector3.Distance(newPath[p0], newPath[p1]);
            Vector3 dir = newPath[p1] - newPath[p0];
            Vector3 normal = Vector3.Normalize(Vector3.Cross(dir, Vector3.up)) * 0.5f;
            if (p1 == 0 || newPath[p0].y != newPath[p1].y || newPath[p0].y > 2f || newPath[p1].y > 2f || p0 - p1 > 3 ||
               (Physics.Raycast(newPath[p0], dir, out hit, dist) && hit.collider.tag == graph.collisionTag) ||
               (Physics.Raycast(newPath[p0] + normal, dir, out hit, dist) && hit.collider.tag == graph.collisionTag) ||
               (Physics.Raycast(newPath[p0] - normal, dir, out hit, dist) && hit.collider.tag == graph.collisionTag)) {
                path.Add(newPath[p1 + 1]);
                p0 = p1;
            }
            p1--;
        }
    }

    public override Vector3 ComputeVelocity() {
        if (this.GetComponent<Communication>().state != StudentState.Wander) {
            targetv = this.GetComponent<Communication>().targetLastKnownPos;
        } else {
            return Vector3.zero;
        }
        // If close enough to target, stop moving
        float distance = Vector3.Distance(targetv, this.transform.position);
        if (distance <= stopRadius) {
            return Vector3.zero;
        }

        if (waitTimer <= 0f) {
            // Run A* to find shortest path to target
            ComputePath(this.transform.position, targetv);
            waitTimer = waitTime;
        } else {
            waitTimer -= 1f * Time.deltaTime;
        }

        // Move towards next node along path
        Vector3 result = Vector3.zero;
        int pathIndex = 0;
        for (; pathIndex < path.Count; pathIndex++) {
            // if we're close enough to the path node, just look for the next one
            if ((path[pathIndex] - this.transform.position).sqrMagnitude < 1f) {
                continue;
            // if the first path node is behind us, get rid of it
            } else if (pathIndex == 0 && path.Count >= 2 && IsBehind(path[0]) && !IsBehind(path[1])) {
                continue;
            }

            // Take vector towards position and project onto xz plane, since character cannot actually move up
            result = path[pathIndex] - this.transform.position;
            result = Vector3.Normalize(result - Vector3.Project(result, Vector3.up));
            break;
        }

        if (pathIndex > 0) {
            path.RemoveRange(0, pathIndex);
        }

        return result;
    }

    // Is the position behind me?
    private bool IsBehind(Vector3 pos) {
        float angle = Mathf.Abs(Vector3.Angle(this.transform.forward, transform.position - pos));
        return (angle < 90 || angle > 270);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        if (path == null) {
            return;
        }
        for (int i = 0; i < path.Count - 1; i++) {
            Gizmos.DrawLine(path[i], path[i+1]);
        }
    }

}