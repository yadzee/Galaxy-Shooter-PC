using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _transition;
    [SerializeField] private Animator _loading;
    [SerializeField] private float _transitionTime = 1f;
    [SerializeField] private GameObject _loadingScreen;

    private void Start()
    {
        _audioSource.Play();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadSinglePlayerMode();
        }

    }
    public void LoadSinglePlayerMode()
    {
        StartCoroutine(LoadAsynchronouslyRoutine(1));
        Time.timeScale = 1;
    }

    public void LoadCoOpMode()
    {
        StartCoroutine(LoadAsynchronouslyRoutine(2));
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronouslyRoutine(int sceneIndex)
    {
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);
        _loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            _loading.SetTrigger("StartLoading");
            yield return null;
        }
    }
}
