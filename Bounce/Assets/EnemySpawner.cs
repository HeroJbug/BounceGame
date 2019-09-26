﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public GameObject player;
    public LayerMask playerMask;
    public float spawnTime = 5f;
    public float checkRadius = 1f;
    float spawnTimeCounter;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimeCounter = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimeCounter -= Time.deltaTime;
        if(spawnTimeCounter <= 0 && CheckSpawnEnemies())
        {
            SpawnEnemies(1);
            spawnTimeCounter = spawnTime;
        }
    }

    private bool CheckSpawnEnemies()
    {
        if(Physics.CheckSphere(transform.position, checkRadius, playerMask))
        {
            return false;
        }
        return true;
    }

    private void SpawnEnemies(int enemyCount)
    {
        GameObject newEnemy = Instantiate(enemyTypes[0], transform.position, new Quaternion(-0.707f, 0, 0, 0.707f));
        newEnemy.GetComponent<Enemy>().target = player.transform;
    }
}
