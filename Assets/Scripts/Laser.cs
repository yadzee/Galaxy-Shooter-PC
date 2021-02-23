using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speedUp;
    [SerializeField] private float _speedDown;
    private bool _isEnemyLaser = false;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_isEnemyLaser == true)
        {
            _speedDown += _spawnManager.EnemySpeedAccel();
        }
    }

    void Update()
    {

        if (_isEnemyLaser == false)
        {
            MoveUp();
        }

        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speedUp * Time.deltaTime);

        if (transform.position.y > 10f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }

    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speedDown * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }

    }

    public void CurrentSprite(int _spriteNumber)
    {
            _spriteRenderer.sprite = _sprites[_spriteNumber];
    }

public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null && player._isPlayerOne == true)
            {
                player.DamagePlayerOne();
            }

            else if (player != null && player._isPlayerTwo == true)
            {
                player.DamagePlayerTwo();
            }

            Destroy(this.gameObject);
        }
    }
}
