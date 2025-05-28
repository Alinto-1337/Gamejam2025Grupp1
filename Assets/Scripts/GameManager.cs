using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using TMPro;



public class GameManager : Singleton<GameManager>
{

    public bool gameStarted = false;

    [SerializeField] UnityEvent onGameStart;
    [SerializeField] PlayableAsset timelineMainMenuIdle;
    [SerializeField] PlayableAsset timelineToPlayOnStart;

    [Header("PlayButton")]
    [SerializeField] TextMeshProUGUI playButtonText;


    private void Start()
    {
        // GetComponent<PlayableDirector>().enabled = true;
        // GetComponent<PlayableDirector>().playableAsset = timelineMainMenuIdle;
        // GetComponent<PlayableDirector>().Play();
    }

    public void StartGame()
    {
        gameStarted = true;
        onGameStart.Invoke();

        GetComponent<PlayableDirector>().playableAsset = timelineToPlayOnStart;
        GetComponent<PlayableDirector>().Play();
        Invoke(nameof(DisablePlayableDirector), 3f);
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
        var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            Destroy(obj);
        }

        Destroy(gameObject);
        gameObject.SetActive(false);
        Instance = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        gameStarted = false;
    }

    public void CorrectTextPosition()
    {
        playButtonText.rectTransform.anchoredPosition = new Vector2(25, 20);
    }

    public void ReturnTextPosition()
    {
        playButtonText.rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    void ReloadScene()
    {
        
    }
    

}