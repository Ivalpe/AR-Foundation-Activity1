using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using TMPro;

public class WaveControl : MonoBehaviour
{
    public Transform player;
    public List<GameObject> enemyPrefabs;

    [Header("Spawning")]
    public float minDistance = 2f;
    public float maxDistance = 5f;
    public float navMeshRadius = 2f;

    [Header("Oleadas")]
    public float timeBetweenWaves = 3f;
    public int baseEnemies = 3;
    public int enemiesIncreasePerWave = 2;

    [Header("UI Debug Info")]
    public TextMeshProUGUI debugText;

    private int currentWave = 0;
    private int maxWave = 3;

    [SerializeField] public ARNavMeshManager navManager;

    private void Start()
    {
        if (player == null && Camera.main != null)
            player = Camera.main.transform;

        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    { 
        //hold wave spawning until nav mesh exists
        if (navManager != null)
        {
            while (!navManager.HasNavMesh)
            {

                Debug.Log("NAVMESH NOT READY");
                debugText.text = "NavMesh State: NOT READY";
                yield return new WaitForSeconds(0.1f);
            }
            debugText.text = "NavMesh State: SET AND READY!";
        }

        do
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
        
        } while (currentWave < maxWave);

        if (currentWave >= maxWave)
        {
            debugText.text = "All waves completed!";
            Debug.Log("All waves completed!");
        }
    }

    private void SpawnEnemy()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
        Vector3 spawnPos = new Vector3(player.position.x + circle.x, player.position.y, player.position.z + circle.y);

        NavMeshHit hit;
        //project position to navmesh floor
        if(NavMesh.SamplePosition(spawnPos, out hit, navMeshRadius, NavMesh.AllAreas))
        {
            spawnPos = hit.position; //snap the coordinates to the exact floor
        }
        else
        {
            if(NavMesh.SamplePosition(spawnPos, out hit, 30.0f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        enemy.tag = "Enemy";

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if(agent != null && navManager.HasNavMesh)
        {
            if (agent.isOnNavMesh)agent.SetDestination(spawnPos);
            else
            {
                NavMeshHit navhit;
                if (NavMesh.SamplePosition(transform.position, out navhit, 30.0f, NavMesh.AllAreas))
                {
                    agent.Warp(navhit.position);
                }
                
            }
        }

        Vector3 lookDir = player.position - spawnPos;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            enemy.transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
