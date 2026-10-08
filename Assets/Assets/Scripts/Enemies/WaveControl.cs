using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using TMPro;
using UnityEngine.InputSystem;

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

    public UIController uiScript;

    public int currentWave = 0;
    private int maxWave = 3;
    public bool win = false;



    [SerializeField] public ARNavMeshManager navManager;


    private void Start()
    {

        if (player == null && Camera.main != null)
            player = Camera.main.transform;
        win = false;
        StartCoroutine(WaveLoop());
    }

    public void Restart()
    {
        win = false;
        currentWave = 0;
    }

    void Update()
    {
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            win = true;

        }

        if (win)
        {
            win = false;
            if (currentWave <= maxWave)
            {
                currentWave = maxWave + 1;

            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                foreach (GameObject e in enemies)
                {
                    Destroy(e);
                }
            }

            if (uiScript)
            {
                uiScript.winScreen.SetActive(true);
                uiScript.audioSource.pitch = 1.0f;
                uiScript.audioSource.PlayOneShot(uiScript.yaySFX);
            }

        }
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
            Debug.Log("NAVMESH  READY");
        }

        currentWave++;
        while (currentWave <= maxWave)
        {



            StartCoroutine(AnnounceNewWave(currentWave));



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

        if (currentWave > maxWave && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            debugText.text = "All waves completed!";
            Debug.Log("All waves completed!");
            win = true;
        }


    }

    private void SpawnEnemy()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
        //int y=1;
        //navManager.GetNavy(y);
        // Debug.Log("navmesh y:"+y);
        Vector3 spawnPos = new Vector3(player.position.x + circle.x, player.position.y, player.position.z + circle.y);


        NavMeshHit hit;
        //project position to navmesh floor
        if (NavMesh.SamplePosition(spawnPos, out hit, navMeshRadius, NavMesh.AllAreas))
        {
            spawnPos = hit.position; //snap the coordinates to the exact floor
        }
        else
        {
            if (NavMesh.SamplePosition(spawnPos, out hit, 30.0f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        enemy.tag = "Enemy";

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null && navManager.HasNavMesh)
        {
            if (agent.isOnNavMesh) agent.SetDestination(spawnPos);
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

    private IEnumerator AnnounceNewWave(int waveNum)
    {
        uiScript.waveText.text = "Wave " + waveNum + " !";
        yield return new WaitForSeconds(3.0f);
        uiScript.waveText.text = "";
    }
}
