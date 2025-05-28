using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;



public class GameManager : Singleton<GameManager>
{

    public bool gameStarted = false;

    [SerializeField] UnityEvent onGameStart;
    [SerializeField] PlayableAsset timelineToPlayOnStart;


    private void Start()
    {

    }

    public void StartGame()
    {
        gameStarted = true;
        onGameStart.Invoke();

        GetComponent<PlayableDirector>().playableAsset = timelineToPlayOnStart;
        GetComponent<PlayableDirector>().Play();
        Invoke(nameof(DisablePlayableDirector), 5f);
    }

    void DisablePlayableDirector()
    {
        GetComponent<PlayableDirector>().enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        gameStarted = false;
    }
}