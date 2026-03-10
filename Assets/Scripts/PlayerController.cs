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

    public bool isMoving, isCrouching, isRunning, isSliding;
    private float X, Y;
    [Header("Headbob Variables")]
    public float headbobSpeed = 10f;
    public float bobbingAmount = 0.5f;
    public CharacterController controller;

    private float timer;
    private Vector3 defaultPosition;

    public void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .75f, 1);
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
        // Useful if you want footstep sounds and or other features in your game.
        isMoving = cc.velocity.sqrMagnitude > 0.0f;
        if (isMoving)
        {
            // Apply a sine function that moves the player camera up and down
            timer += Time.deltaTime * headbobSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosition.y + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else if (isRunning)
        {
            float v = headbobSpeed * 2;
        }
        else
        {
            
            // if player is not moving, then move player camera back to default positions
            isMoving = false;
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosition.y, Time.deltaTime * headbobSpeed), transform.localPosition.z);
        }
    }
}