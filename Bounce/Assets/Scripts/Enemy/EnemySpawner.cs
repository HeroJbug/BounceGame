using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject SpawnEnemy(GameObject toSpawn, GameObject player)
    {
        GameObject newEnemy = Instantiate(toSpawn, transform.position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().target = player.transform;
        return newEnemy;
    }
}
