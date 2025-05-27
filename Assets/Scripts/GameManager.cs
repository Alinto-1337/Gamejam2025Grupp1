using UnityEngine;
using UnityEngine.Events;



public class GameManager : Singleton<GameManager>
{

    public bool gameStarted = false;

    [SerializeField] UnityEvent onGameStart;


    private void Start()
    {

    }

    public void StartGame()
    {
        gameStarted = true;
        onGameStart.Invoke();
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