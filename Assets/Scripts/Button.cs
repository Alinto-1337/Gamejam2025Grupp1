using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] buttonIconStages;
    [SerializeField] GameObject buttonPresser;
    [SerializeField] GameObject glassDome;

    [SerializeField] AudioSource CalmMusic;
    [SerializeField] AudioSource HyperMuic;
    [SerializeField] AudioSource ButtonLidPlopp;

    [SerializeField] GameObject Explosion;

    [SerializeField] float[] buttonHeightPos;
    [SerializeField] float blinkInterval;
    [SerializeField] float gravityStrength;

    int currentStage = 0;

    bool lidPopped = false;

    float blinkTimer;

    private void Start()
    {
        UpdateUI();
    }

    public void TakeDamage()
    {
        EffectManager.Instance.PlayScreenShakePulse(0.8f, EffectManager.EffectPower.aggressive);
        EffectManager.Instance.PlayVignettePulse(0.5f, Color.red, EffectManager.EffectPower.normal);

        if (currentStage > 1)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            return;
        }

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
        buttonPresser.transform.localPosition -= Vector3.up * 0.005f; 
    }

    public void PopTheLid()
    {
        glassDome.GetComponent<Rigidbody>().isKinematic = false;
        glassDome.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(-15, 15), Random.Range(30, 40), Random.Range(-15, 15));
        ButtonLidPlopp.PlayOneShot(ButtonLidPlopp.clip);
        lidPopped = true;
    }
    void Gravity()
    {
        if (!lidPopped) return;
        
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
                CalmMusic.Stop();
                HyperMuic.Play();

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
