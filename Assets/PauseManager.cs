using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] Animator PauseMenuAnimator;
    [SerializeField] string appearAnimName;
    [SerializeField] string disappearAnimName;

    float standardTimeScale = 1f;
    void Start()
    {
        PauseMenuAnimator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        Time.timeScale = 0f;
        bool isPaused = PauseMenuAnimator.gameObject.activeInHierarchy;
        PauseMenuAnimator.gameObject.SetActive(!isPaused);

        PauseMenuAnimator.Play(isPaused ? disappearAnimName : appearAnimName);
        Time.timeScale = standardTimeScale;
    }
}
