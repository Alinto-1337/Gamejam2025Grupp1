using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, MyInputManager.IPlayerActions
{
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSmoothing;
    [SerializeField] float maxDistance;
    [SerializeField] float walkCycle;
    [SerializeField] GameObject footsteps;
    [SerializeField] GameObject SFX;

    [Header("Audio")]
    [SerializeField] AudioClip [] footstepsClip;
    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] string walkAnimName;
    [SerializeField] string idleAnimName;

    AudioSource source;
    
    private float Timer;

    Vector2 input;

    Vector3 velocity;
    Vector3 vel;

    bool isAiming = false;
    bool isWalking = false;

    Rigidbody rb;


    MyInputManager.PlayerActions playerActions;

    Vector2 moveDirX = new Vector2(Mathf.Sqrt(2), -Mathf.Sqrt(2));
    Vector2 moveDirY = new Vector2(Mathf.Sqrt(2), Mathf.Sqrt(2));

    private void Start()
    {
        playerActions = new MyInputManager().Player;
        playerActions.Enable();
        playerActions.SetCallbacks(this);

        rb = GetComponent<Rigidbody>();

        source = GetComponent<AudioSource>();

        if (animator != null)
        {
            animator.CrossFade(idleAnimName, 0.1f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.gameStarted) { return; }
        input = context.ReadValue<Vector2>();
    }

    void WalkAudio()
    {
        AudioClip randFoot = footstepsClip[Random.Range(0, footstepsClip.Length)];

        //float randPitch = Random.Range(0.8f, 1.2f);

        //source.pitch = randPitch;

        GameObject footSFX = Instantiate(SFX, transform.position, Quaternion.identity);

        //footSFX.GetComponent<AudioSource>().pitch = randPitch;
        footSFX.GetComponent<AudioSource>().clip = randFoot;
        footSFX.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        // --- Movement
        Vector2 direction = input.x * moveDirX + input.y * moveDirY;

        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);

        velocity = Vector3.SmoothDamp(velocity, moveDir, ref vel, movementSmoothing);

        rb.linearVelocity = velocity * movementSpeed;
        
        bool wasWalking = isWalking;
        isWalking = velocity.magnitude > 0.1f;

        if (isWalking != wasWalking)
        {
            if (isWalking)
            {
                if (animator != null)
                {
                    animator.CrossFade(walkAnimName, 0.1f);
                }
            }
            else
            {
                if (animator != null)
                {
                    animator.CrossFade(idleAnimName, 0.1f);
                }
            }
        }

        // --- Rotation
        if (isAiming)
        {
            Vector3 aimDir = WorldSpaceGursor.Instance.transform.position - transform.position;
            aimDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDir), 0.2f);
        }
        else
        {
            // Look/rotate in the y axis in the direction the player is moving
            Vector3 flatVel = new Vector3(velocity.x, 0, velocity.z);
            if (flatVel.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flatVel), 0.2f);
        }

        // --- Walk cycle for footstep sfx
        Timer += Time.deltaTime;

        if (Timer > walkCycle && velocity.magnitude > 1)
        {
            Instantiate(footsteps, transform.position, footsteps.transform.rotation);
            Timer = 0;
            WalkAudio();
        }

        // --- Clamp position to circle
        if (Vector3.Distance(Vector3.up, transform.position) > maxDistance)
        {
            transform.position = (transform.position - Vector3.up).normalized * maxDistance + Vector3.up;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
    }
}
