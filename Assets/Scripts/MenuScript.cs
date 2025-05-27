using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] Animator[] animators;
    bool isPaused;

    private void Awake()
    {
        isPaused = true;
    }

    public void Play()
    {
        animators[0].SetBool("PlayIsPressed", true);
        animators[1].SetBool("PlayIsPressed", true);
        animators[2].SetBool("PlayIsPressed", true);
        isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& isPaused == false)
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
    }

    public void Resume()
    {
        animators[3].SetBool("ESCisPressed", false);
        isPaused = false;
    }
}