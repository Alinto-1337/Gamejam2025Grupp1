using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, MyInputManager.IPlayerActions
{
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSmoothing;
    [SerializeField] AudioSource moveAudio;


    Vector2 input;

    Vector3 velocity;
    Vector3 vel;


    MyInputManager.PlayerActions playerActions;

    Vector2 moveDirX = new Vector2(Mathf.Sqrt(2), -Mathf.Sqrt(2));
    Vector2 moveDirY = new Vector2(Mathf.Sqrt(2), Mathf.Sqrt(2));

    private void Start()
    {
        playerActions = new MyInputManager().Player;
        playerActions.Enable();
        playerActions.SetCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        if (context.performed && input.magnitude > 0.1f)
        {
            if (!moveAudio.isPlaying)
                moveAudio.Play();
        }
        else if (context.canceled || input.magnitude <= 0.1f)
        {
            if (moveAudio.isPlaying)
                moveAudio.Pause(); // Or Stop() if needed
        }
    }

    private void Update()
    {
        Vector2 direction = input.x * moveDirX + input.y * moveDirY;

        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);

        velocity = Vector3.SmoothDamp(velocity, moveDir, ref vel, movementSmoothing);

        transform.position += velocity * movementSpeed * Time.deltaTime;
    }
}
