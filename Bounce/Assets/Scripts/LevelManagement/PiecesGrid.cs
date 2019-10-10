using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesGrid : MonoBehaviour
{
    LevelNode[,] grid;
    public Vector2 gridWorldSize;
    public bool displayGridGizmos;
    public LayerMask barrierMask;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    private EnemySpawner[] spawnerObjects;

    public float nodeRadius;

    float nodeDiam;
    int gridSizeX;
    int gridSizeY;
    private Vector3 worldBottomLeft;

    private void Awake()
    {
        nodeDiam = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiam);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiam);

        spawnerObjects = GameObject.FindObjectsOfType<EnemySpawner>();

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new LevelNode[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPt = worldBottomLeft + Vector3.right * (x * nodeDiam + nodeRadius) + Vector3.up * (y * nodeDiam + nodeRadius);
                bool isOccupied = (Physics.CheckSphere(worldPt, nodeRadius, barrierMask));
                grid[x, y] = new LevelNode(isOccupied, worldPt, x, y);
            }
        }
    }

    public Vector2 GetGridSize()
    {
        return new Vector2(gridSizeX, gridSizeY);
    }

    public bool IsOccupied(int x, int y)
    {
        return grid[x, y].occupied;
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return grid[x, y].worldPos;
    }

    public void UpdateGridLocation(int x, int y, GameObject newOccupant)
    {
        grid[x, y].SetOccupant(newOccupant);
        //CheckForAndDestroyEnemies(x, y);
    }

    public LevelNode GetNode(Vector2Int loc)
    {
        return grid[loc.x, loc.y];
    }

    //FIXME
    private void CheckForAndDestroyEnemies(int x, int y)
    {
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        Vector3 worldPt = worldBottomLeft + Vector3.right * (x * nodeDiam + nodeRadius) + Vector3.up * (y * nodeDiam + nodeRadius);
        RaycastHit[] hits = Physics.SphereCastAll(worldPt, nodeRadius, Vector3.forward, enemyMask);
        foreach(RaycastHit enemyHit in hits)
        {
            Destroy(enemyHit.rigidbody.gameObject);
        }
    }

    public bool CheckForObstaclesAtLoc(int x, int y)
    {
        return CheckForPlayer(x, y) && CheckForSpawner(x, y);
    }

    private bool CheckForSpawner(int x, int y)
    {
        foreach(EnemySpawner sp in spawnerObjects)
        {
            if(Mathf.RoundToInt(sp.gameObject.transform.position.x) == x && Mathf.RoundToInt(sp.gameObject.transform.position.y) == y)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckForPlayer(int x, int y)
    {
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        Vector3 worldPt = worldBottomLeft + Vector3.right * (x * nodeDiam + nodeRadius) + Vector3.up * (y * nodeDiam + nodeRadius);
        return Physics2D.OverlapCircle(worldPt, nodeRadius, playerMask);
    }

    private void OnDrawGizmos()
    {
        if(displayGridGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
            if (grid != null)
            {
                foreach (LevelNode n in grid)
                {
                    Gizmos.color = (n.occupied) ? Color.red : Color.white;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiam - 0.1f));
                }
            }
        }
    }
}
