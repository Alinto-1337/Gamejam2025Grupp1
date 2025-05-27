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

    public void HoveringPlay()
    {
        animators[1].SetBool("IsHovering", true);
    }

    public void HoveringQuit()
    {
        animators[2].SetBool("IsHovering", true);
    }

    public void Quit()
    {
        Application.Quit();
        animators[2].SetTrigger("IsClicked");
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
        animators[4].SetTrigger("isClicked");
    }

    public void Exit()
    {
        animators[0].SetBool("PlayIsPressed", false);
        animators[1].SetBool("PlayIsPressed", false);
        animators[2].SetBool("PlayIsPressed", false);
        animators[3].SetBool("ESCisPressed", false);
        animators[6].SetTrigger("IsClicked");

        inMenu = true;
    }

    public void Resume()
    {
        animators[3].SetBool("ESCisPressed", false);
        animators[5].SetTrigger("IsClicked");
        isPaused = false;
    }

    

    public void HoverResume()
    {
        animators[5].SetBool("isHovering", true);
    }

    public void HoverRestart()
    {
        animators[4].SetBool("isHovering", true);
    }

    public void HoverExit()
    {
        animators[6].SetBool("isHovering", true);
    }

    public void ExitHover()
    {
        animators[1].SetBool("IsHovering", false);
        animators[2].SetBool("IsHovering", false);
        animators[4].SetBool("isHovering", false);
        animators[5].SetBool("isHovering", false);
        animators[6].SetBool("isHovering", false);
    }    
}