using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tutorial Credit to: https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=1 - Sebastian Lague

public class AStarPathfinding : MonoBehaviour
{
    public Transform seeker, target;
    PathfindingGrid grid;

    private void Awake()
    {
        grid = this.GetComponent<PathfindingGrid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodePosFromWorldPoint(startPos);
        Node targetNode = grid.NodePosFromWorldPoint(targetPos);

        PathfindingHeap<Node> openSet = new PathfindingHeap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.HeapCount > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                TracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbor in grid.GetNeighbors(currentNode))
            {
                if(!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newMoveCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(newMoveCost < neighbor.gCost || !openSet.Contains(neighbor))
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

    void TracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;
        while(current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();

        grid.path = path;
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
