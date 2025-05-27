using UnityEngine;



public class GameManager : Singleton<GameManager>
{

    public bool gameStarted = false;


    private void Start()
    {
        
    }


    public void StartGame()
    {
        gameStarted = true;
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