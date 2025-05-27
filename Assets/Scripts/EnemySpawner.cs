using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] GameObject[] enemyPrefabs;

    [SerializeField] float spawnDistance;
    [SerializeField] float spawnDistanceX;
    [SerializeField] float spawnDistanceZ;
    [SerializeField] float[] spawnInterval;
    [SerializeField] float timeConstant;

    [SerializeField] float enemyInstanceAmmount = 5;
    int enemyInstances;
    private void Start()
    {
        StartCoroutine(spawnEnemyA());
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

        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity).GetComponent<EnemieBehavior>().SetTarget(button.transform);
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
    IEnumerator spawnEnemyA ()
    {
        SpawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(Random.Range(-spawnDistanceX, spawnDistanceX), 0, Random.Range(-spawnDistanceZ, spawnDistanceZ)));
        yield return new WaitForSeconds(spawnInterval[0]);
        HandleEnemyATimer();
        StartCoroutine(spawnEnemyA());
    }
}
