using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl : MonoBehaviour
{
    public Transform player;
    public List<GameObject> enemyPrefabs;

    [Header("Spawning")]
    public float minDistance = 10f;
    public float maxDistance = 15f;

    [Header("Oleadas")]
    public float timeBetweenWaves = 3f;
    public int baseEnemies = 3;
    public int enemiesIncreasePerWave = 2;

    private int currentWave = 0;

    private void Start()
    {
        if (player == null && Camera.main != null)
            player = Camera.main.transform;

        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave++;
            int countToSpawn = baseEnemies + (currentWave - 1) * enemiesIncreasePerWave;

            for (int i = 0; i < countToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.6f);
            }

            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
        Vector3 spawnPos = new Vector3(player.position.x + circle.x, player.position.y, player.position.z + circle.y);

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        enemy.tag = "Enemy";

        Vector3 lookDir = player.position - spawnPos;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            enemy.transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
