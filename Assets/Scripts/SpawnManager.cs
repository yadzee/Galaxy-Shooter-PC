using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _medKitPrefab;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private float _enemySpeedAcceleration;
    


    private bool _stopSpawning = false;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnMedkitRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (_stopSpawning == false)
        {

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 20f));
        }
    }

    IEnumerator SpawnMedkitRoutine()
    {
        yield return new WaitForSeconds(Random.Range(15f, 40f));

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_medKitPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15f, 40f));
        }
    }



    public float EnemySpeedAccel()
    {
        if (_enemySpeedAcceleration <= 5f)
        {
            _enemySpeedAcceleration += 0.005f;
        }
        return _enemySpeedAcceleration;
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
