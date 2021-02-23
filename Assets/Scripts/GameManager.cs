using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver = false;
    [SerializeField] private bool _isGamePaused = false;
    public bool _isCoOpMode;
    public bool _isPlayerOneAlive;
    public bool _isPlayerTwoAlive;


    [SerializeField] private GameObject _pauseMenuPanel;

    public void Update()
    {
        GameOver();
    }

    public void ResumeGame()
    {
        _isGamePaused = false;
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void IsGameOver()
    {
        _isGameOver = true;
    }

    public void GameOver()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            if (_isCoOpMode == false)
            {
                SceneManager.LoadScene(1);// Single Player
            }

            else
            {
                SceneManager.LoadScene(2);// Co-Op
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _isGamePaused == false)
        {
            _isGamePaused = true;
            _pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && _isGamePaused == true)
        {
            ResumeGame();
        }

    }

}
