using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<EnemyWave> enemyWaves = new List<EnemyWave>();
    public EnemyWave currentWave;
    public int nextWaveIndex = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnWave(enemyWaves[nextWaveIndex]);
            currentWave = enemyWaves[nextWaveIndex];
            nextWaveIndex += 1;
        }
    }

    public void SpawnWave(EnemyWave ew)
    {
        for (int i = 0; i < ew.enemyAmount; i++)
        {
            SpawnEnemy(ew.waveEnemies[Random.Range(0, ew.waveEnemies.Count)], ew);
        }
    }

    public void SpawnEnemy(Enemy enemy, EnemyWave wave)
    {
        var e = Instantiate(enemy, wave.waveSpawnPositions[Random.Range(0, wave.waveSpawnPositions.Count)].position, Quaternion.identity);
        e.SetTarget(FindFirstObjectByType<FPSController>().transform);
        wave.currentWaveEnemies.Add(e);
    }

    public void DespawnWave()
    {
        if (currentWave != null && currentWave.currentWaveEnemies.Count > 0)
        {
            for (int i = currentWave.currentWaveEnemies.Count - 1; i == 0; i--)
            {
                Destroy(currentWave.currentWaveEnemies[i].gameObject);
            }
        }
    }
}

[System.Serializable]
public class EnemyWave
{
    public int enemyAmount;
    public List<Enemy> waveEnemies = new List<Enemy>();
    public List<Transform> waveSpawnPositions = new List<Transform>();
    public List<Enemy> currentWaveEnemies = new List<Enemy>();
}