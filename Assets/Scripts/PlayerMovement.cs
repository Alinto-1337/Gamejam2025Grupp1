using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, MyInputManager.IPlayerActions
{
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSmoothing;
    [SerializeField] float walkCyckle;
    [SerializeField] GameObject footsteps;
    [SerializeField] GameObject SFX;

    [Header("Audio")]
    [SerializeField] AudioClip [] footstepsClip;

    AudioSource source;
    
    private float Timer;

    Vector2 input;

    Vector3 velocity;
    Vector3 vel;

    bool isAiming = false;


    MyInputManager.PlayerActions playerActions;

    Vector2 moveDirX = new Vector2(Mathf.Sqrt(2), -Mathf.Sqrt(2));
    Vector2 moveDirY = new Vector2(Mathf.Sqrt(2), Mathf.Sqrt(2));

    private void Start()
    {
        playerActions = new MyInputManager().Player;
        playerActions.Enable();
        playerActions.SetCallbacks(this);

        source = GetComponent<AudioSource>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    void WalkAudio()
    {
        AudioClip randFoot = footstepsClip[Random.Range(0, footstepsClip.Length)];

        float randPitch = Random.Range(0.8f, 1.2f);

        source.pitch = randPitch;

        GameObject footSFX = Instantiate(SFX, transform.position, Quaternion.identity);

        footSFX.GetComponent<AudioSource>().pitch = randPitch;
        footSFX.GetComponent<AudioSource>().clip = randFoot;
        footSFX.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        // --- Movement
        Vector2 direction = input.x * moveDirX + input.y * moveDirY;

        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);

        velocity = Vector3.SmoothDamp(velocity, moveDir, ref vel, movementSmoothing);

        transform.position += velocity * movementSpeed * Time.deltaTime;


        // --- Rotation
        if (isAiming)
        {

        }
        else
        {
            
        }

        // --- Walk cycle for footstep sfx
        Timer += Time.deltaTime;

        if (Timer > walkCyckle && velocity.magnitude > 1)
        {
            Instantiate(footsteps, transform.position, footsteps.transform.rotation);
            Timer = 0;
            WalkAudio();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        
    }
}
