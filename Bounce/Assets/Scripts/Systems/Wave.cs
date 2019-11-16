using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave")]
public class Wave : ScriptableObject
{
    public List<TypeToCount> enemyTypesToCounts;
}

[System.Serializable]
public struct TypeToCount
{
    public GameObject enemyType;
    public int enemyCount;
}
