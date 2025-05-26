using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, MyInputManager.IGunActions
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [Tooltip("Rounds Per Minute")]
    [SerializeField] float firerate;
    [SerializeField] int maxAmmo;
    float firerateS;

    int ammo;

    bool shooting = false;

    float shootTimer;

    MyInputManager.GunActions gunActions;

    private void Start()
    {
        gunActions = new MyInputManager().Gun;
        gunActions.Enable();
        gunActions.SetCallbacks(this);

        firerateS = 1 / (firerate/60);

        ammo = maxAmmo;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shooting = true;
            Debug.Log("grui");
        }
        if (context.canceled)
        {
            shooting = false;
        }
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shooting && shootTimer > firerateS)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("ShootingPlane")))
            {
                GameObject bulletInstace = Instantiate(bullet, transform.position, Quaternion.identity);
                
                bulletInstace.GetComponent<Rigidbody>().linearVelocity = (hit.point-transform.position).normalized * bulletSpeed;

                shootTimer = 0;
            }
        }
    }
}
