using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float speed, walk, run, crouch;

    private Vector3 crouchScale, normalScale;

    public bool isMoving, isCrouching, isRunning, isGrounded;
    private float X, Y;
    public float jumpHeight = 15.0f;
    public float fallSpeed = -10.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundmask;
    public AudioClip jumpSound;
    private Vector3 velocity;


    private float timer;
    private Vector3 defaultPosition;

    public void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .50f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        #region Camera Limitation Calculator
        //Camera limitation variables
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;
        #endregion
        transform.localRotation = Quaternion.Euler(Y, X, 0.0f);

        // Calculates if player is on the ground using the groundCheck gameobject
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;

            cc.SimpleMove(Vector3.Normalize(forward + right) * speed);
        // Determines if the speed = run or walk
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run;
            isRunning = true;
        }
        //Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
            speed = walk;
            player.transform.localScale = normalScale;
        }
        // Detects if the player is moving.
        // Useful for footstep sounds.
        isMoving = cc.velocity.sqrMagnitude > 0.0f;

        // Jumping mechanic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * fallSpeed);
        }

        // If player is in the air they fall
        velocity.y += fallSpeed * Time.deltaTime;

    }

}
