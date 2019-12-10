using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private EnemySpawner[] spawners;
    private GameObject[] players = new GameObject[2];
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
    //can make this a hashset later for more efficiency if need be
    private List<GameObject> currentWaveEnemies;

    private bool waveFinished = true;
    public GameObject nextWaveApproachingUI;

    private void OnEnable()
    {
        Enemy.EnemyDeathEvent += RemoveDeadEnemies;
    }

    private void OnDisable()
    {
        Enemy.EnemyDeathEvent -= RemoveDeadEnemies;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnTimeCounter = spawnTime;
        spawners = FindObjectsOfType<EnemySpawner>();
        FirstRound();
        FindPlayers();
    }

    public void FindPlayers()
    {
        PlayerMovement[] temp = FindObjectsOfType<PlayerMovement>();
        for (int i = 0; i < temp.Length; i++)
        {
            players[i] = temp[i].gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveFinished)
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
        foreach (EnemySpawner s in spawners)
        {
            //if we can spawn something, do so
            if (enemiesStack.Count > 0)
            {
                GameObject toSpawn = enemiesStack.Pop();
                GameObject spawned;
                if (players[1] != null)
                    spawned = s.SpawnEnemy(toSpawn, players[Random.Range(0, 2)]);
                else
                    spawned = s.SpawnEnemy(toSpawn, players[0]);
                currentWaveEnemies.Add(spawned);
            }
        }
    }

    private void RemoveDeadEnemies(GameObject e)
    {
        if (currentWaveEnemies.Count > 1)
            currentWaveEnemies.Remove(e);
        else
        {
            currentWaveEnemies.Remove(e);
            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextRound()
    {
        print("NEXT ROUND STARTED");
        //turn on UI element
        yield return new WaitForSeconds(timeBetweenRounds);
        //turn off UI element

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
        nextWaveApproachingUI.SetActive(true);
        yield return new WaitForSeconds(timeBetweenWaves);
        if (waveIdx < currRound.wavesInRound.Count)
        {
            nextWaveApproachingUI.SetActive(false);

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
        currentWaveEnemies = new List<GameObject>();
        foreach (TypeToCount e in currWaveInRound.enemyTypesToCounts)
        {
            for (int i = 0; i < e.enemyCount; i++)
            {
                enemiesStack.Push(e.enemyType);
            }
        }
    }
}