using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] Animator[] animators;
    bool isPaused;
    bool inMenu;

    private void Awake()
    {
        isPaused = false;
        inMenu = true;
    }

    public void Play()
    {
        animators[0].SetBool("PlayIsPressed", true);
        animators[1].SetBool("PlayIsPressed", true);
        animators[2].SetBool("PlayIsPressed", true);
        inMenu = false;
    }

    public void Quit()
    {
        Application.Quit();
    }    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& !isPaused && !inMenu)
        {
            animators[3].SetBool("ESCisPressed", true);
            isPaused = true;            
        }
        else if (Input.GetKeyDown(KeyCode.Escape)&& isPaused == true)
        {
            animators[3].SetBool("ESCisPressed", false);
            isPaused = false;
        }        
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        animators[0].SetBool("PlayIsPressed", false);
        animators[1].SetBool("PlayIsPressed", false);
        animators[2].SetBool("PlayIsPressed", false);
        animators[3].SetBool("ESCisPressed", false);
        inMenu = true;
    }

    public void Resume()
    {
        animators[3].SetBool("ESCisPressed", false);
        isPaused = false;
    }
}