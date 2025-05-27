using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, MyInputManager.IPlayerActions
{
    [SerializeField] float movementSpeed;
    [SerializeField] float movementSmoothing;
    [SerializeField] float walkCyckle;
    [SerializeField] GameObject footsteps;
    
    private float Timer;

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
    }

    private void Update()
    {
        Vector2 direction = input.x * moveDirX + input.y * moveDirY;

        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);

        velocity = Vector3.SmoothDamp(velocity, moveDir, ref vel, movementSmoothing);

        transform.position += velocity * movementSpeed * Time.deltaTime;

        Timer += Time.deltaTime;

        if (Timer > walkCyckle && velocity.magnitude > 1)
        {
            Instantiate(footsteps, transform.position, footsteps.transform.rotation);
            Timer = 0;
        }

        Debug.Log(Timer);
    }
}
