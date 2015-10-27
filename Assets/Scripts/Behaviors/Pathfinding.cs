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
	public GameObject target;
    public Graph graph;

    private List<Vector3> path;
    private float waitTimer = 0f;

    private void Awake() {
        path = new List<Vector3>();
    }

    // Heuristic function for A*. Use Euclidean distance for now
    private float HeuristicValue(Node3D node, Node3D targetNode) {
        return Vector3.Distance(graph.WorldPosition(node, 0f), graph.WorldPosition(targetNode, 0f));
    }

    // Run A* and update the path of nodes we want to travel
    private void ComputePath(Vector3 startPos, Vector3 targetPos) {
        Node3D startNode = graph.NearestNode(startPos);
        Node3D targetNode = graph.NearestNode(targetPos);

        HashSet<Node3D> visited = new HashSet<Node3D>();
        PriorityQueue<float, Node3D> frontier = new PriorityQueue<float, Node3D>();
        frontier.Enqueue(0f, startNode);

        // initialize map of parents (in-edge neighbor to each vertex) for path reconstruction
        Node3D[,] parents = new Node3D[graph.xgrid + 1, graph.zgrid + 1];
        parents[startNode.x, startNode.z] = startNode;

        // initialize costs
        float[,] costs = new float[graph.xgrid + 1, graph.zgrid + 1];
        for (int i = 0; i < graph.xgrid; i++) {
            for (int j = 0; j < graph.zgrid; j++) {
                if (i == startNode.x && j == startNode.z) {
                    costs[i, j] = 0f;
                } else {
                    costs[i, j] = -1f;
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

                float cost = costs[current.x, current.z] + graph.dist;
                // If we found a better cost/distance, add to frontier
                // TODO: use new Tuple data type instead of these hacky int casts
                if (costs[neighbors[i].x, neighbors[i].z] < 0f || cost < costs[neighbors[i].x, neighbors[i].z]) {
                    costs[neighbors[i].x, neighbors[i].z] = cost;
                    parents[neighbors[i].x, neighbors[i].z] = current;
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
        path.Clear();
        current = targetNode;
        path.Add(graph.WorldPosition(current, this.transform.position.y));
        while (current != startNode) {
            current = parents[current.x, current.z];
            path.Add(graph.WorldPosition(current, this.transform.position.y));
        }
        path.Reverse();
    }

    public override Vector3 ComputeVelocity() {
        // If close enough to target, stop moving
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        if (distance <= stopRadius) {
            return Vector3.zero;
        }

        if (waitTimer <= 0f) {
            // Run A* to find shortest path to target
            ComputePath(this.transform.position, target.transform.position);
            waitTimer = waitTime;
        } else {
            waitTimer -= 1f * Time.deltaTime;
        }

        // Move towards next node along path
        while (path.Count > 0) {
            // if we're close enough to the path node, just look for the next one
            if ((path[0] - this.transform.position).sqrMagnitude < 1f) {
                path.RemoveAt(0);
            // if the first path node is behind us, get rid of it
            } else if (path.Count >= 2 && IsBehind(path[0]) && !IsBehind(path[1])) {
                path.RemoveAt(0);
            } else {
                return Vector3.Normalize(path[0] - this.transform.position);
            }
        }

        return Vector3.zero;
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