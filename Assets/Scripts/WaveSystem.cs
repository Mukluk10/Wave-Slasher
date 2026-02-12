using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemiesToSpawm;

    [SerializeField] private int enemyCount;
    public int enemiesKilled;
    [SerializeField] private int spawnedEnemies;

    [SerializeField] private int waveCount;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float timeBetweenEnemies;
    [SerializeField] private bool inDownTime;

    private bool spawningEnemy;
    private bool waveIsStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(WaveDowntime());
    }

    private void FixedUpdate()
    {
        if (!inDownTime)
        {
            if (spawnedEnemies < enemyCount && !spawningEnemy)
            {
                StartCoroutine(SpawnEnemyDelay());
            }
        }

        if(enemiesKilled == spawnedEnemies && waveIsStarted)
        {
            inDownTime = true;
            waveIsStarted = false;
            StartCoroutine(WaveDowntime());
        }
    }

    private void WaveStart()
    {
        waveCount++;
        enemyCount += 3;
        enemiesKilled = 0;
        spawnedEnemies = 0;
        inDownTime = false;
    }

    private IEnumerator WaveDowntime()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        WaveStart();
    }

    private IEnumerator SpawnEnemyDelay()
    {
        spawningEnemy = true;
        yield return new WaitForSeconds(timeBetweenEnemies);
        Debug.Log("EnemySpawned");
        spawnedEnemies++;
        spawningEnemy = false;
        waveIsStarted = true;
    }
}
