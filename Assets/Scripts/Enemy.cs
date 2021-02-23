using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _enemySoundClip;
    [SerializeField] private AudioClip _enemyLaserClip;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private bool isEnemyAlive;

    private SpawnManager _spawnManager;
    private Animator _animator;
    private UI_Manager _uiManager;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private Player _player;



    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }

        _animator = gameObject.GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("The Animator is Null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The audio source on the enemy is Null");
        }

        else
        {
            _audioSource.clip = _enemySoundClip;
        }

        _speed += _spawnManager.EnemySpeedAccel();
        
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && isEnemyAlive)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_enemyLaserClip);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            

            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].CurrentSprite(1);
                lasers[i].AssignEnemyLaser();
            }

        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(Random.Range(-9.3f, 9.3f), 10, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            _audioSource.Play();

            if (player != null && player._isPlayerOne == true)
            {
                _uiManager.Score(10);
                player.DamagePlayerOne();
            }

            else if (player != null && player._isPlayerTwo == true)
            {
                _uiManager.Score(10);
                player.DamagePlayerTwo();
            }

            _speed = 0;

            _animator.SetTrigger("OnEnemyDeath");

            isEnemyAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _audioSource.Play();

            if (_player != null)
            {
               _uiManager.Score(10);
            }

            _speed = 0;

            _animator.SetTrigger("OnEnemyDeath");

            isEnemyAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
