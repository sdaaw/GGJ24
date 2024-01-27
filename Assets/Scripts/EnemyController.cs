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
            SpawnEnemy(ew.waveEnemies[Random.Range(0, ew.waveEnemies.Count - 1)], ew);
        }
    }

    public void SpawnEnemy(WaveEnemy enemy, EnemyWave wave)
    {
        var e = Instantiate(enemy.enemyPrefab, enemy.spawnPos);
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
    public List<WaveEnemy> waveEnemies = new List<WaveEnemy>();
    public List<Enemy> currentWaveEnemies = new List<Enemy>();
}

[System.Serializable]
public class WaveEnemy
{
    public Enemy enemyPrefab;
    public Transform spawnPos;
}
