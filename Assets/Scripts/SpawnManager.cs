using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private int _powerUpCount; //counts how many before missels are released
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerupRoutine());
    }
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            _powerUpCount++;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            if (_powerUpCount >= 10)
            {
                _powerUpCount = 0;
                Instantiate(powerups[5], posToSpawn, Quaternion.identity);
                float waitTime = Random.Range(3f, 7f);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                int randomPowerUp = Random.Range(0, 5);
                Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
                float waitTime = Random.Range(3f, 7f);
                yield return new WaitForSeconds(waitTime);
            } 
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
