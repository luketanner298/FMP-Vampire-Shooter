using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;

    [SerializeField] private float speed, walk, run, crouch;

    private Vector3 crouchScale, normalScale;
    public float jumpForce = 50000000f; // Jump force
    public LayerMask groundLayer; // Layer for ground detection
    public float groundCheckDistance = 1f; // Distance for raycast to check for ground
    public float Gravity;
    public bool isMoving, isCrouching, isRunning, isGrounded;
    public GameObject orientation;

    private float X, Y;
    private Rigidbody rb;
    
    private void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .50f, 1);
        normalScale = new Vector3(1, 1, 1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
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

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 forward = (orientation.transform.forward * vertical) ;
        Vector3 right = orientation.transform.right * horizontal;
        orientation.transform.position = gameObject.transform.position;
        orientation.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
        // Ground check using a raycast
        //isGrounded = Physics.Raycast(gameObject.transform.position + new Vector3(0,-1,0), Vector3.down, groundCheckDistance, groundLayer);
        foreach (RaycastHit GroundRaycast in Physics.RaycastAll(gameObject.transform.position + new Vector3(0, -1, 0), Vector3.down, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        //Debug.DrawRay(gameObject.transform.position + new Vector3(0, -1, 0), Vector3.down, Color.red);

        rb.velocity =(Vector3.Normalize(forward + right) * speed) + new Vector3(0, rb.velocity.y, 0);
        //Debug.Log("rigidbody velocity =" + rb.velocity);
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
        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //Debug.Log("Character should jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        // Detects if the player is moving.
        // Useful if you want footstep sounds and or other features in your game.
        isMoving = rb.velocity.sqrMagnitude > 0.0f;
            // Visualize raycast for ground check in editor
    }
}