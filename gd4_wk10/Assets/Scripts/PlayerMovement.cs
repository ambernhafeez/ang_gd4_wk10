using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float runMultiplier = 1.5f;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float gravity = 10f;
    public float mouseSensitivity = 2f;
    [SerializeField] float lookXlimit = 60f;
    float rotationX = 0;
    Vector3 moveDirection;
    CharacterController controller;
    public float stamina = 100f;
    public float staminaDrainSpeed = 20f;
    public Image staminaBar;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;

        Cursor.visible = false;
    }

    void Update()
    {
        #region Movement
        if(controller.isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // preserve y velocity of the player, if player is grounded
            float movementDirectionY = moveDirection.y;

            moveDirection = (horizontalInput * transform.right) + (verticalInput * transform.forward);

            if(Input.GetButtonDown("Jump"))
            // hardcoding jumping rather than relying on rigidbodies 
            {
                moveDirection.y = jumpForce;
            }
            else 
            {
                // sharper fall by setting moveDirection to the last preserved y velocity
                // faster than waiting for gravity to reduce moveDirection.y
                moveDirection.y = movementDirectionY;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // sprint mechanic
        if(Input.GetKeyDown(KeyCode.LeftShift) && stamina > 0) 
        { 
            moveSpeed *= runMultiplier;
        }

        if(Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            // reduce stamina while key is pressed
            stamina -= Time.deltaTime * staminaDrainSpeed;
            staminaBar.fillAmount = stamina / 100f;
        }
        else
        {
            // recover stamina over time if sprint button is not pressed
            if(stamina < 100)
            {
                stamina += Time.deltaTime * staminaDrainSpeed;
                staminaBar.fillAmount = stamina / 100f;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            moveSpeed /= runMultiplier;
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);


        #endregion

        #region Rotation
        // allow camera to rotate up and down
        transform.Rotate(Vector3.up * mouseSensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));

        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        // check if rotation is trying to go outside of the look limit, stop it using clamp
        rotationX = Mathf.Clamp(rotationX, -lookXlimit, lookXlimit);

        // change local rotation to rotationX while keeping other axes at 0
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);


        #endregion
    }
}
