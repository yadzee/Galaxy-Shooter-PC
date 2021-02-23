using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Text _scoreText, _scoreBestText;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _liveImgPlayerOne;
    [SerializeField] private Image _liveImgPlayerTwo;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1f;
    [SerializeField] private int _score;
    [SerializeField] private int _bestScore;

    private AudioFade _audioFade;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;

    void Start()
    {
        _audioFade = GameObject.Find("Audio_Manager").GetComponentInChildren<AudioFade>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is Null");
        }

        if (_gameManager._isCoOpMode == false)
        {
            _bestScore = PlayerPrefs.GetInt("HighScore", 0);
            _scoreBestText.text = "Best: " + _bestScore;
        }
        else
        {
            _bestScore = PlayerPrefs.GetInt("HighScoreCoop", 0);
            _scoreBestText.text = "Best: " + _bestScore;
        }
    }


    public void ScoreUI(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }


    public void UpdateLivesPlayerOne(int currentLives)
    {
        _liveImgPlayerOne.sprite = _liveSprites[currentLives];

        if (currentLives <= 0 && _gameManager._isCoOpMode == false)
        {
            GameOver();
        }

        else if (currentLives <= 0 && _gameManager._isCoOpMode == true)
        {
            if(_gameManager._isPlayerOneAlive == false && _gameManager._isPlayerTwoAlive == false)
            {
                GameOver();
            }
        }
    }

    public void UpdateLivesPlayerTwo(int currentLives)
    {
        _liveImgPlayerTwo.sprite = _liveSprites[currentLives];

        if (currentLives <= 0 && _gameManager._isCoOpMode == false)
        {
            GameOver();
        }

        else if (currentLives <= 0 && _gameManager._isCoOpMode == true)
        {
            if (_gameManager._isPlayerOneAlive == false && _gameManager._isPlayerTwoAlive == false)
            {
                GameOver();
            }
        }
    }

    public void Score(int points)
    {
        _score += points;
        ScoreUI(_score);

        switch (_score)
        {
            case 300:
                _audioFade.FadeOut();
                break;

            case 800:
                _audioFade.FadeOut();
                break;

            case 1200:
                _audioFade.FadeOut();
                break;

            case 1600:
                _audioFade.FadeOut();
                break;

        }
    }

    public void CheckForBestScore()
    {
        if (_score > _bestScore && _gameManager._isCoOpMode == false)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
            _scoreBestText.text = "Best: " + _bestScore;
        }

        else if (_score > _bestScore && _gameManager._isCoOpMode == true)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScoreCoop", _bestScore);
            _scoreBestText.text = "Best: " + _bestScore;
        }
    }

    public void GameOver()
    {
        _spawnManager.OnPlayerDeath();
        GameOverSequence();
    }


    void GameOverSequence()
    {
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.IsGameOver();

    }

    public void ResumePlay()
    {
        _gameManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        StartCoroutine(LoadLevel(0));
        Time.timeScale = 1;
    }

    public void RestartMenuSingle()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void RestartMenuCoop()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    public void ApllicationQuit()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

        }
    }

}
    
