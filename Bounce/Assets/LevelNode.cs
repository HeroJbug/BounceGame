using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode
{
    public bool occupied;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    private GameObject occupant;

    public LevelNode(bool _occupied, Vector3 _worldPos, int _gridX, int _gridY)
    {
        occupied = _occupied;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        occupant = null;
    }

    public void SetOccupant(GameObject newOccupant)
    {
        occupant = newOccupant;
        occupied = true;
    }

    public GameObject GetOccupant()
    {
        return occupant;
    }
}
