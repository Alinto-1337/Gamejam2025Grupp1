using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] buttonIconStages;
    [SerializeField] GameObject buttonPresser;
    [SerializeField] GameObject glassDome;

    [SerializeField] float[] buttonHeightPos;
    [SerializeField] float blinkInterval;
    [SerializeField] float gravityStrength;

    int currentStage = 2;

    float blinkTimer;

    private void Start()
    {
        UpdateUI();
        PopTheLid();
    }

    public void TakeDamage()
    {
        currentStage++;
        UpdateUI();
        UpdateButtonPosition();
    }

    void UpdateUI()
    {
        buttonImage.sprite = buttonIconStages[currentStage];
    }

    void UpdateButtonPosition()
    {
        buttonPresser.transform.localPosition = new Vector3(buttonPresser.transform.position.x, buttonHeightPos[currentStage], buttonPresser.transform.position.z);
    }

    void PopTheLid()
    {
        glassDome.GetComponent<Rigidbody>().isKinematic = false;
        glassDome.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(-15, 15), Random.Range(30, 40), Random.Range(-15, 15));
    }
    void Gravity()
    {
        Vector3 gravity = new Vector3(0, -gravityStrength, 0);

        glassDome.GetComponent<Rigidbody>().linearVelocity += gravity * Time.deltaTime;

    }
    private void Update()
    {
        if (currentStage > 1)
        {
            blinkTimer += Time.deltaTime;

            if (currentStage == 2 && blinkTimer > blinkInterval)
            {
                currentStage++;

                UpdateUI();

                blinkTimer = 0;
            }

            if (currentStage >= 3 && blinkTimer > blinkInterval)
            {
                currentStage = 2;
                UpdateUI();

                blinkTimer = 0;
            }
        }
        Gravity();
    }
}
