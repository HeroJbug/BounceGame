﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPiecesController : MonoBehaviour
{
    PiecesGrid levelGrid;
    //TODO: Change to list so every new piece may be spawned
    public List<GameObject> possibleHazards;
    public float timeBetweenSpawns;
    public float timeBetweenRemovals;
    public PathfindingGrid pathGrid;
    float spawnTimerCount;
    float removalTimerCount;
    int piecesCount;
    List<Vector2Int> occupiedGridLocations;
    Vector2Int lastAdded;

    private void Awake()
    {
        levelGrid = this.GetComponent<PiecesGrid>();
        spawnTimerCount = timeBetweenSpawns;
        removalTimerCount = timeBetweenRemovals;
        occupiedGridLocations = new List<Vector2Int>();
    }

    private void Update()
    {
        spawnTimerCount -= Time.deltaTime;
        removalTimerCount -= Time.deltaTime;
        if(spawnTimerCount <= 0)
        {
            SpawnNewPiece();
            spawnTimerCount = timeBetweenSpawns;
        }
        if(removalTimerCount <=0 && piecesCount > 7)
        {
            RemovePiece();
            removalTimerCount = timeBetweenRemovals;
        }
    }

    private void SpawnNewPiece()
    {
        if(lastAdded != null)
        {
            //this is here so that the thing that was just added is excluded from the random choice
            occupiedGridLocations.Add(lastAdded);
        }

        int locX = Random.Range(0, (int)levelGrid.GetGridSize().x);
        int locY = Random.Range(0, (int)levelGrid.GetGridSize().y);

        //keep recalculating randoms if we find occupied locations
        while(levelGrid.IsOccupied(locX, locY) || levelGrid.CheckForObstaclesAtLoc(locX, locY))
        {
            locX = Random.Range(0, (int)levelGrid.GetGridSize().x);
            locY = Random.Range(0, (int)levelGrid.GetGridSize().y);
        }

        //spawn and update that that location is now occupied. Also update our pathfinding grid
        GameObject newPiece = ChooseNextPiece();
        GameObject instantiatedPiece = Instantiate(newPiece, levelGrid.GetWorldPos(locX, locY), Quaternion.identity);
        levelGrid.UpdateGridLocation(locX, locY, instantiatedPiece);
        lastAdded = new Vector2Int(locX, locY);
        StartCoroutine(RemapPathfind());
        piecesCount++;
    }

    private GameObject ChooseNextPiece()
    {
        //can add weights later if we want
        int randomSelect = Random.Range(0, possibleHazards.Count);
        //if it's a bomb don't count it as an added piece, preemptivley decrement
        Hazard newH = possibleHazards[randomSelect].GetComponent<Hazard>();
        if (newH != null)
        {
            if (newH.Type == HazardTypes.BOMB)
                piecesCount--;
        }
        return possibleHazards[randomSelect];
    }

    private void RemovePiece()
    {
        int randomRemoveLoc = Random.Range(0, occupiedGridLocations.Count);
        LevelNode toMod = levelGrid.GetNode(occupiedGridLocations[randomRemoveLoc]);
        Destroy(toMod.GetOccupant());
        toMod.occupied = false;
        StartCoroutine(RemapPathfind());
        piecesCount--;
    }

    IEnumerator RemapPathfind()
    {
        yield return new WaitForSeconds(2f);
        pathGrid.MapGrid();
        yield return null;
    }
}
