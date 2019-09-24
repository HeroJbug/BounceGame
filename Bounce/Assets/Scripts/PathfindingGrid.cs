using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public Transform playerPos;
    public LayerMask barrierMask;
    Node[,] grid;
    public Vector2 gridWrldSize;
    public float nodeRadius;

    float nodeDiam;
    int gridSizeX;
    int gridSizeY;

    private void Start()
    {
        nodeDiam = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWrldSize.x / nodeDiam);
        gridSizeY = Mathf.RoundToInt(gridWrldSize.y / nodeDiam);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWrldSize.x / 2 - Vector3.up * gridWrldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPt = worldBottomLeft + Vector3.right * (x * nodeDiam + nodeRadius) + Vector3.up * (y * nodeDiam + nodeRadius);
                bool canTraverse = !(Physics.CheckSphere(worldPt, nodeRadius, barrierMask));
                grid[x, y] = new Node(canTraverse, worldPt);
            }
        }
    }

    public Node NodePosFromWorldPoint(Vector3 worldPos)
    {
        float percentX = Mathf.Clamp01((worldPos.x + gridWrldSize.x / 2) / gridWrldSize.x);
        float percentY = Mathf.Clamp01((worldPos.y + gridWrldSize.y / 2) / gridWrldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWrldSize.x, gridWrldSize.y, 1));

        if(grid != null)
        {
            Node playerNode = NodePosFromWorldPoint(playerPos.position);
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(playerNode == n)
                {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiam - 0.1f));
            }
        }
    }
}
