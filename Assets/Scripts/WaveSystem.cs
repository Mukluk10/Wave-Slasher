using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemiesToSpawm;

    [SerializeField] private int enemyCount;
    public int enemiesKilled;
    private int waveCount;
    private int spawnedEnemies;
    [SerializeField] private float timeBetweenWaves;

    [SerializeField] private bool inDownTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(WaveDowntime());
    }

    private void Update()
    {
        if (!inDownTime)
        {
            for (spawnedEnemies = 0; spawnedEnemies < enemyCount; spawnedEnemies++)
            {
                Debug.Log("EnemySpawned");
            }

            inDownTime = true;
            StartCoroutine(WaveDowntime());
        }
    }

    private void WaveStart()
    {
        waveCount++;
        enemyCount += 3;
        inDownTime = false;
    }

    private IEnumerator WaveDowntime()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        WaveStart();
    }

}
