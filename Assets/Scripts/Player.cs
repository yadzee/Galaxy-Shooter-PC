using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotGreenPrefab;
    [SerializeField] private GameObject _tripleShotBluePrefab;
    [SerializeField] private GameObject _tripleShotRedPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private int _livesPlayerOne = 3;
    [SerializeField] private int _livesPlayerTwo = 3;
    [SerializeField] private GameObject _shieldVisualiser;
    [SerializeField] private GameObject _leftEngineDamage;
    [SerializeField] private GameObject _rightEngineDamage;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _lossClip;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private Material _matWhite;
    [SerializeField] private Material _matDefault;

    private Laser _laser;
    private SpawnManager _spawnManager;
    private UI_Manager _uiManager;
    private GameManager _gameManager;
    private SpriteRenderer _sr;


    [SerializeField] public bool _isPlayerOne;
    [SerializeField] public bool _isPlayerTwo;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _IsShieldIsActive = false;


    void Start()
    {
        _leftEngineDamage.SetActive(false);
        _rightEngineDamage.SetActive(false);
        _laser = _laserPrefab.GetComponent<Laser>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        _sr = GetComponent<SpriteRenderer>();
        _matWhite = Resources.Load("White_mat", typeof(Material)) as Material;
        _matDefault = _sr.material;
        

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The audio source on the player is Null");
        }

        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    void Update()
    {
        if (_isPlayerOne == true)
        {
            CalculateMovement();
            if ((Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) && _isPlayerOne == true)
            {
                FireLaser();
            }
        }

        if (_isPlayerTwo == true)
        {
            PlayerTwoMovement();
            if ((Input.GetKeyDown(KeyCode.KeypadEnter) && Time.time > _canFire) && _isPlayerTwo == true)
            {
                FireLaserPlayerTwo(); 
            }
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 6)
        {
            transform.position = new Vector3(transform.position.x, 6, 0);
        }

        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.x >= 11.15f)
        {
            transform.position = new Vector3(-11.24f, transform.position.y, 0);
        }

        else if (transform.position.x <= -11.24f)
        {
            transform.position = new Vector3(11.15f, transform.position.y, 0);
        }

    }

    void PlayerTwoMovement()
    {
        float horizontalInput = Input.GetAxis("HorizontalPlayerTwo");
        float verticalInput = Input.GetAxis("VerticalPlayerTwo");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 6)
        {
            transform.position = new Vector3(transform.position.x, 6, 0);
        }

        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.x >= 11.15f)
        {
            transform.position = new Vector3(-11.24f, transform.position.y, 0);
        }

        else if (transform.position.x <= -11.24f)
        {
            transform.position = new Vector3(11.15f, transform.position.y, 0);
        }

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_gameManager._isCoOpMode == true)
        {
            _laser.CurrentSprite(0);

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotBluePrefab, transform.position, Quaternion.identity);
            }

            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.35f, 0), Quaternion.identity);
            }

        }
        else
        {
            _laser.CurrentSprite(2);

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotGreenPrefab, transform.position, Quaternion.identity);
            }

            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.35f, 0), Quaternion.identity);
            }

        }


        _audioSource.Play(0);

    }

    void FireLaserPlayerTwo()
    {
        _canFire = Time.time + _fireRate;
        _laser.CurrentSprite(3);

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotRedPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.35f, 0), Quaternion.identity);

        }

        _audioSource.Play(0);

    }


    public void DamagePlayerOne()
    {

        if (_IsShieldIsActive == true)
        {
            _IsShieldIsActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }

        _livesPlayerOne--;
        _sr.material = _matWhite;
        AudioSource.PlayClipAtPoint(_lossClip, transform.position, 1.0f);


        if (_livesPlayerOne == 2)
        {
            Invoke("ResetMaterial", 0.1f);
            _leftEngineDamage.SetActive(true);
        }

        else if (_livesPlayerOne == 1)
        {
            Invoke("ResetMaterial", 0.1f);
            _rightEngineDamage.SetActive(true);
        }


        else if (_livesPlayerOne < 1)
        {
            _livesPlayerOne = 0;
            _gameManager._isPlayerOneAlive = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.CheckForBestScore();
            Destroy(this.gameObject);
        }

        _uiManager.UpdateLivesPlayerOne(_livesPlayerOne);


    }

    public void DamagePlayerTwo()
    {

        if (_IsShieldIsActive == true)
        {
            _IsShieldIsActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }

        _livesPlayerTwo--;
        _sr.material = _matWhite;
        AudioSource.PlayClipAtPoint(_lossClip, transform.position, 1.0f);


        if (_livesPlayerTwo == 2)
        {
            Invoke("ResetMaterial", 0.1f);
            _leftEngineDamage.SetActive(true);
        }

        else if (_livesPlayerTwo == 1)
        {
            Invoke("ResetMaterial", 0.1f);
            _rightEngineDamage.SetActive(true);
        }


        else if (_livesPlayerTwo < 1)
        {
            _livesPlayerTwo = 0;
            _gameManager._isPlayerTwoAlive = false;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.CheckForBestScore();
            Destroy(this.gameObject);
        }

        _uiManager.UpdateLivesPlayerTwo(_livesPlayerTwo);

    }

    public void AddHealthPLayerOne()
    {
        if (_livesPlayerOne == 3)
        {
            _livesPlayerOne = 3;
        }
        else if (_livesPlayerOne == 2)
        {
            _leftEngineDamage.SetActive(false);
            _livesPlayerOne++;
        }
        else if (_livesPlayerOne == 1)
        {
            _rightEngineDamage.SetActive(false);
            _livesPlayerOne++;
        }
        _uiManager.UpdateLivesPlayerOne(_livesPlayerOne);
    }

    public void AddHealthPlayerTwo()
    {
        if (_livesPlayerTwo == 3)
        {
            _livesPlayerTwo = 3;
        }
        else if (_livesPlayerTwo == 2)
        {
            _leftEngineDamage.SetActive(false);
            _livesPlayerTwo++;
        }
        else if (_livesPlayerTwo == 1)
        {
            _rightEngineDamage.SetActive(false);
            _livesPlayerTwo++;
        }
        _uiManager.UpdateLivesPlayerTwo(_livesPlayerTwo);
    }


    void ResetMaterial()
    {
        _sr.material = _matDefault;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotDownRoutine());
    }

    IEnumerator TripleShotDownRoutine()
    {
        while (_isTripleShotActive == true)
        {

            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;

        }
    }


    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }


    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldBoostActive()
    {
        _IsShieldIsActive = true;
        _shieldVisualiser.SetActive(true);
    }
}


