using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] frozenStartEnemies;

    [SerializeField] float spawnDistance;
    [SerializeField] float spawnDistanceX;
    [SerializeField] float spawnDistanceZ;
    [SerializeField] float[] spawnInterval;
    [SerializeField] float timeConstant;

    [SerializeField] float enemyInstanceAmmount = 5;
    int enemyInstances;

    private void Start()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs assigned in EnemySpawner script.");
            return;
        }

        if (frozenStartEnemies.Length > 0)
        {
            foreach (GameObject enemy in frozenStartEnemies)
            {
                enemy.SetActive(false);
            }
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(spawnEnemyA());

        if (frozenStartEnemies.Length > 0)
        {
            foreach (GameObject enemy in frozenStartEnemies)
            {
                enemy.SetActive(true);
            }
        }
    }
   /* void SpawnEnemy (GameObject enemyPrefab, Vector3 spawnPoint, int valueCheck)
    {
         Debug.Log(valueCheck);
         if (valueCheck == 0) 
         {
             if (spawnPoint.x < button.transform.position.x)
             {
                 spawnPoint.x = button.transform.position.x - spawnDistanceX;
             }
             else
             {
                 spawnPoint.x = button.transform.position.x + spawnDistanceX;
             }
         }
         else if (valueCheck == 1)
         {
             if (spawnPoint.z < button.transform.position.z)
             {
                 spawnPoint.z = button.transform.position.z - spawnDistanceZ;
             }
             else
             {
                 spawnPoint.z = button.transform.position.z + spawnDistanceZ;
             }
         }
        
    } */

    void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPoint)
    {
        Vector2 buttonTransform = new Vector2(button.transform.position.x, button.transform.position.z);
        Vector2 spawning = Random.insideUnitCircle.normalized;
        spawnPoint.x = button.transform.position.x + spawning.x;
        spawnPoint.z = button.transform.position.z + spawning.y;
        spawnPoint *= spawnDistance;

        if (enemyPrefab == enemyPrefabs[2])
        {
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity).GetComponent<BombEnemyBehaviour>().SetTarget(player.transform);
        }
        else
        {
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity).GetComponent<EnemieBehavior>().SetTarget(button.transform);
        }
    }

    void HandleEnemyATimer()
    {
        enemyInstances += 1;
        if (enemyInstances > enemyInstanceAmmount)
        {
            spawnInterval[0] = spawnInterval[0] * timeConstant;
            enemyInstances = 0;
        }
    }
    IEnumerator spawnEnemyA()
    {
        SpawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(Random.Range(-spawnDistanceX, spawnDistanceX), 0, Random.Range(-spawnDistanceZ, spawnDistanceZ)));
        yield return new WaitForSeconds(spawnInterval[0]);
        HandleEnemyATimer();
        StartCoroutine(spawnEnemyA());
    }
}
