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
        animators[0].SetBool("IsPlaying", false);
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
        animators[4].SetTrigger("isClicked");
    }

    public void Exit()
    {        
        animators[3].SetBool("ESCisPressed", false);
        animators[6].SetTrigger("IsClicked");

        inMenu = true;
    }

    public void Resume()
    {
        animators[3].SetBool("ESCisPressed", false);        
        isPaused = false;
    }
}