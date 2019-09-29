using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPiecesController : MonoBehaviour
{
    PiecesGrid levelGrid;
    //TODO: Change to list so every new piece may be spawned
    public GameObject barrier;
    public float timeBetweenSpawns;
    public PathfindingGrid pathGrid;
    float spawnTimerCount;
    int piecesCount;
    List<Vector2Int> occupiedGridLocations;

    private void Awake()
    {
        levelGrid = this.GetComponent<PiecesGrid>();
        spawnTimerCount = timeBetweenSpawns;
        occupiedGridLocations = new List<Vector2Int>();
    }

    private void Update()
    {
        spawnTimerCount -= Time.deltaTime;
        if(spawnTimerCount <= 0)
        {
            if(piecesCount > 5)
            {
                RemovePiece();
            }
            SpawnNewPiece();
            spawnTimerCount = timeBetweenSpawns;
        }
    }

    private void SpawnNewPiece()
    {
        int locX = Random.Range(0, (int)levelGrid.GetGridSize().x);
        int locY = Random.Range(0, (int)levelGrid.GetGridSize().y);

        //keep recalculating randoms if we find occupied locations
        while(levelGrid.IsOccupied(locX, locY) || levelGrid.CheckForPlayer(locX, locY))
        {
            locX = Random.Range(0, (int)levelGrid.GetGridSize().x);
            locY = Random.Range(0, (int)levelGrid.GetGridSize().y);
        }

        //spawn and update that that location is now occupied. Also update our pathfinding grid
        GameObject newPiece = Instantiate(barrier, levelGrid.GetWorldPos(locX, locY), Quaternion.identity);
        levelGrid.UpdateGridLocation(locX, locY, newPiece);
        occupiedGridLocations.Add(new Vector2Int(locX, locY));
        pathGrid.ReMapGrid();
        piecesCount++;
    }

    private void RemovePiece()
    {
        int randomRemoveLoc = Random.Range(0, occupiedGridLocations.Count);
        LevelNode toMod = levelGrid.GetNode(occupiedGridLocations[randomRemoveLoc]);
        Destroy(toMod.GetOccupant());
        toMod.occupied = false;
        piecesCount--;
    }
}
