using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _speedRotate;
    [SerializeField] private SpawnManager _spawnManager;


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        Movement();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(GetComponent<Collider2D>());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            Destroy(GetComponent<Collider2D>());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            if (player != null && player._isPlayerOne == true)
            {
                player.DamagePlayerOne();
            }

            else if (player != null && player._isPlayerTwo == true)
            {
                player.DamagePlayerTwo();
            }

            _spawnManager.StartSpawning();
            Destroy(this.gameObject);

        }
    }

    public void Movement()
    {
        transform.Rotate(Vector3.forward * _speedRotate * Time.deltaTime);
    }
}
