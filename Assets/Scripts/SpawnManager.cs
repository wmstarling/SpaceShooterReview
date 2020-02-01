using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private int _powerUpCount; //counts how many before missels are released
    [SerializeField]
    private int[] _waveCount;
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UIManager Null on Spawn Manager");
        }
    }
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
            for(int i = 0; i< _waveCount.Length; i++)
            {
                _uiManager.UpdateWave((i+1));   
                for(int j = 0; j < _waveCount[i]; j++)
                {
                    int randomEnemy = Random.Range(0, 5);
                    Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject newEnemy = Instantiate(_enemies[randomEnemy], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                    yield return new WaitForSeconds(5.0f);
                }
                yield return new WaitForSeconds(6.0f);
            }
            //once this exits it is the final wave
            //spawn the boss
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            _powerUpCount++;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            if (_powerUpCount >= 5)
            {
                _powerUpCount = 0;
                int randomRare = Random.Range(6,8);
                Instantiate(_powerups[randomRare], posToSpawn, Quaternion.identity);
                float waitTime = Random.Range(3f, 7f);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                int randomPowerUp = Random.Range(0, 5);
                Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
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
