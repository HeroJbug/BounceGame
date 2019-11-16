using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private EnemySpawner[] spawners;
    private GameObject player;
    public List<Round> rounds;
    public float timeBetweenWaves;
    public float timeBetweenRounds;
    //private float waveTimeInternal;
    Round currRound;
    Wave currWaveInRound;
    int waveIdx = 0;
    int roundIdx = 0;
    public float spawnTime = 5f;
    float spawnTimeCounter;
    private Stack<GameObject> enemiesStack;

    private bool waveFinished = true;
    //private bool roundFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimeCounter = spawnTime;
        spawners = FindObjectsOfType<EnemySpawner>();
        FirstRound();
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!waveFinished)
            spawnTimeCounter -= Time.deltaTime;
        if (spawnTimeCounter <= 0)
        {
            SpawnEnemies();
            spawnTimeCounter = spawnTime;
        }

        //if(waveFinished)
        //{
        //    waveTimeInternal -= Time.deltaTime;
        //    if(waveTimeInternal <= 0)
        //    {
        //        waveTimeInternal = timeBetweenWaves;
        //        waveFinished = false;
        //        StartNextWave();
        //    }
        //}
    }

    private void FirstRound()
    {
        currRound = rounds[roundIdx];
        waveIdx = 0;
        roundIdx++;
        StartCoroutine(StartNextWave());
    }

    private void SpawnEnemies()
    {
        print("SPAWNING ENEMIES");
        //loop through all spawners
        foreach(EnemySpawner s in spawners)
        {
            //if we can spawn something, do so
            if(enemiesStack.Count > 0)
            {
                GameObject toSpawn = enemiesStack.Pop();
                s.SpawnEnemy(toSpawn, player);
            }
            //otherwise Start the next wave
            else
            {
                waveFinished = true;
                StartCoroutine(StartNextWave());
            }
        }
    }

    private IEnumerator StartNextRound()
    {
        print("NEXT ROUND STARTED");
        yield return new WaitForSeconds(timeBetweenRounds);
        //Link UI update here

        currRound = rounds[roundIdx];
        waveIdx = 0;
        StartCoroutine(StartNextWave());
        if (roundIdx < rounds.Count - 1)
            roundIdx++;
        else
            Debug.LogError("No More Rounds");
    }

    private IEnumerator StartNextWave()
    {
        print("NEXT WAVE STARTED");
        yield return new WaitForSeconds(timeBetweenWaves);
        if(waveIdx < currRound.wavesInRound.Count)
        {
            //link UI update here

            currWaveInRound = currRound.wavesInRound[waveIdx];
            waveIdx++;
            FillEnemyStack();
            waveFinished = false;
        }
        else
        {
            StartCoroutine(StartNextRound());
        }
    }

    private void FillEnemyStack()
    {
        enemiesStack = new Stack<GameObject>();
        foreach(TypeToCount e in currWaveInRound.enemyTypesToCounts)
        {
            for(int i = 0; i < e.enemyCount; i++)
            {
                enemiesStack.Push(e.enemyType);
            }
        }
    }
}
