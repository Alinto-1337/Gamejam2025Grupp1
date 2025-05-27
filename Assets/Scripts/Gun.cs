using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, MyInputManager.IGunActions
{
    [SerializeField] TMP_Text ammoText;
    [SerializeField] GameObject bullet;
    [SerializeField] float spawnDistance;
    [SerializeField] float bulletSpeed;
    [Tooltip("Rounds Per Minute")]
    [SerializeField] float firerate;
    [SerializeField] int maxAmmo;
    [SerializeField] float reloadTime;
    float firerateS;

    [SerializeField] GameObject MuzzleFlash;

    [Header("Audio")]
    [SerializeField] AudioClip bass;
    [SerializeField] AudioClip[] fire;
    [SerializeField] AudioClip[] mech;

    int ammo;

    bool shooting = false;

    bool reloading;

    float shootTimer;

    MyInputManager.GunActions gunActions;

    AudioSource source;

    private void Start()
    {
        gunActions = new MyInputManager().Gun;
        gunActions.Enable();
        gunActions.SetCallbacks(this);

        source = GetComponent<AudioSource>();

        firerateS = 1 / (firerate/60);

        ammo = maxAmmo;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shooting = true;
        }
        if (context.canceled)
        {
            shooting = false;
        }
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shooting && shootTimer > firerateS && !reloading)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("ShootingPlane")))
            {
                GameObject bulletInstance = Instantiate(bullet, transform.position + Vector3.up + (hit.point - (transform.position + Vector3.up)).normalized * spawnDistance, Quaternion.identity);

                bulletInstance.GetComponent<Rigidbody>().linearVelocity = (hit.point - (transform.position + Vector3.up)).normalized * bulletSpeed;

                bulletInstance.transform.LookAt(bulletInstance.transform.position + bulletInstance.GetComponent<Rigidbody>().linearVelocity.normalized);

                shootTimer = 0;

                // ----

                GameObject Muzzleflash1 = Instantiate(MuzzleFlash, transform.position + Vector3.up, transform.rotation);

                Muzzleflash1.transform.LookAt(transform.position + Vector3.up + (hit.point - (transform.position - Vector3.up)).normalized);

                EffectManager.Instance.PlayScreenShakePulse(.1f, EffectManager.EffectPower.aggressive);

                FireAudio();

                ammo--;

                ammoText.text = ammo + " / " + maxAmmo;

                if (ammo <= 0)
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    void FireAudio()
    {
        AudioClip randFire = fire[Random.Range(0, fire.Length)];
        AudioClip randMech = mech[Random.Range(0, mech.Length)];

        source.PlayOneShot(bass);
        source.PlayOneShot(randFire);
        source.PlayOneShot(randMech);
    }

    IEnumerator Reload()
    {
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        ammo = maxAmmo;
        ammoText.text = ammo + " / " + maxAmmo;

        reloading = false;
    }
}
