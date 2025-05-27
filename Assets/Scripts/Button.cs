using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] buttonIconStages;
    [SerializeField] float blinkInterval;

    int currentStage = 0;

    float blinkTimer;

    private void Start()
    {
        UpdateUI();
    }

    public void TakeDamage()
    {
        currentStage++;
        UpdateUI();
    }

    void UpdateUI()
    {
        buttonImage.sprite = buttonIconStages[currentStage];
    }

    private void Update()
    {
        if (currentStage > 1)
        {
            blinkTimer += Time.deltaTime;

            if (blinkTimer > blinkInterval)
            {

            }
        }
    }
}
