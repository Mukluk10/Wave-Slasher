using System.Collections;
using TMPro;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public static WaveSystem Instance { get; private set; }

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemiesToSpawn;

    [SerializeField] private int enemyCount;
    public int enemiesKilled;
    [SerializeField] private int spawnedEnemies;

    [SerializeField] private int waveCount;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float timeBetweenEnemies;
    [SerializeField] private bool inDownTime;

    private bool spawningEnemy;
    private bool waveIsStarted;

    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI waveCountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        StartCoroutine(WaveDowntime());
        Instance = this; 
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
        Debug.Log("Wave " + waveCount + " started!");
        waveCount++;
        enemyCount += 3;
        enemiesKilled = 0;
        spawnedEnemies = 0;
        anim.SetTrigger("Start");
        waveCountText.text = ("Made it to Wave  " + waveCount);
        inDownTime = false;
    }

    private IEnumerator WaveDowntime()
    {
        anim.SetTrigger("End");
        yield return new WaitForSeconds(timeBetweenWaves);
        WaveStart();
    }

    private IEnumerator SpawnEnemyDelay()
    {
        spawningEnemy = true;
        yield return new WaitForSeconds(timeBetweenEnemies);

        Transform spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate (enemiesToSpawn[0], spawnPos.position, Quaternion.identity);

        spawnedEnemies++;
        spawningEnemy = false;
        waveIsStarted = true;
    }
}
