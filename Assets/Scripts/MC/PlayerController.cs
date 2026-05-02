using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Animator anim;
    public AudioSource audioSource;
    public AudioClip steps;

    [Header("Movement")]
    public float velocity = 1f;
    public PlayerInput playerInput;
    private InputAction moveAction;
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private Vector3 moveDirection = Vector3.zero;
    public float stepTime = 0.5f;
    public bool isMoving;
    private float count;
    private float gravity = -9.81f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        moveAction = playerInput.actions["Move"];

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveInput = new(input.x, 0f, input.y);
        moveInput = transform.TransformDirection(moveInput) * velocity;
        characterController.Move(moveInput * Time.deltaTime);

        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        characterController.Move(playerVelocity * Time.deltaTime);

        if(characterController.isGrounded && moveAction.IsPressed())
        {
            isMoving = true;
            count += Time.deltaTime;
            //if(count >= stepTime)
            //{
                //audioSource.PlayOneShot(steps);
                //count = 0;
            //}
        }
        else
        {
            isMoving = false;
        }
        playerVelocity.y += gravity * Time.deltaTime;

        bool movingForward = input.y > 0.1f;
        bool movingBackward = input.y < -0.1f;
        bool movingRight = input.x > 0.1f;
        bool movingLeft = input.x < -0.1f;

        anim.SetBool("Up", movingForward);
        anim.SetBool("Down", movingBackward);
        anim.SetBool("Right", movingRight);
        anim.SetBool("Left", movingLeft);
    }
}
