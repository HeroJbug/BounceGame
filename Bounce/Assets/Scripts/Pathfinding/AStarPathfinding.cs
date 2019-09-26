using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Tutorial Credit to: https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=1 - Sebastian Lague

public class AStarPathfinding : MonoBehaviour
{
    EnemyPathRequestManager reqManager;
    PathfindingGrid grid;

    private void Awake()
    {
        grid = this.GetComponent<PathfindingGrid>();
        reqManager = GetComponent<EnemyPathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        Node startNode = grid.NodePosFromWorldPoint(startPos);
        Node targetNode = grid.NodePosFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            PathfindingHeap<Node> openSet = new PathfindingHeap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.HeapCount > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathFound = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    int newMoveCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMoveCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCost;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
        }
        yield return null;
        if(pathFound)
        {
            waypoints = TracePath(startNode, targetNode);
        }
        reqManager.FinishedProcessing(waypoints, pathFound);
    }

    Vector3[] TracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;
        while(current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }
        Vector3[] waypoints = TrimPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] TrimPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dirOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 dirNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(dirNew != dirOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            dirOld = dirNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        else
            return 14 * dstX + 10 * (dstY - dstX);
    }
}
