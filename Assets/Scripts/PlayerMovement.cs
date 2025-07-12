//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Serialized fields for easy assignment in the Unity Inspector


    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float sprintMultiplier = 2.0f; // Sprint multiplier

    [SerializeField] private TMP_Text sprintStatusText; // Reference to the TMP_Text for sprint status
    [SerializeField] private Image sprintIcon; // Reference to the Image component for the sprint icon

    public Camera firstPersonCamera; // Reference to the first-person camera
    public Camera thirdPersonCamera; // Reference to the third-person camera

    public int countEnemy; // Count of destroyed enemies
    public TMP_Text EnemyDestroyedText; // Text to display the number of enemies destroyed

    public Vector3 Opponentposition; // Position of the opponent

    private bool isCursorLocked = true; // To manage cursor lock state
    private bool isDragging = false; // For handling mouse drag state
    private bool isSprinting = false; // To manage sprint state

    public AudioClip normalFootstepClip; // For normal walking
    public AudioClip sprintFootstepClip; // For sprinting
    public AudioSource audioSource; // AudioSource component for playing sounds


    //References
    Rigidbody rb; // Rigidbody for physics-based movement
    GameManager gameManager; // Reference to the GameManager
    AudioManager audioManager; // Reference to the AudioManager
    Spawner spawner; // Reference to the Spawner
    Animator animator; // Reference to the Animator


    private enum CameraMode
    {
        FirstPerson,
        ThirdPerson
    }
    private CameraMode currentMode = CameraMode.FirstPerson;

    void Start()
    {
        // Ensure the script is enabled
        this.enabled = true;

        // Initialize Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Get references to GameManager and Spawner
        gameManager = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is null!");
        }

        spawner = GameObject.FindObjectOfType<Spawner>();
        if (spawner == null)
        {
            Debug.LogError("Spawner reference is null!");
        }

        // Update EnemyDestroyedText if it's assigned
        if (EnemyDestroyedText != null)
        {
            EnemyDestroyedText.text = "Enemies Destroyed: " + countEnemy;
        }
        else
        {
            Debug.LogError("EnemyDestroyedText reference is null!");
        }

        // Check for camera references
        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Debug.LogError("Camera references are not set!");
        }

        // Set up cursor and camera position
        UnlockCursor();
        UpdateCameraPosition();

        // Initialize AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the player!");
        }

        // Get reference to AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }

        // Initialize Animator component
        animator = GetComponent<Animator>(); // Get the Animator component
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from the player!");
        }

    }

    void Update()
    {
        // Handle toggling cursor lock and switching camera modes
        HandleCursorLockToggle();
        HandleCameraModeSwitch();


        // If the cursor is locked, handle camera rotation based on input
        if (isCursorLocked)
        {
            RotateCamera();
        }

        // If the cursor is not locked and right mouse button is pressed, start dragging for camera rotation
        else if (Input.GetMouseButtonDown(1)) //right click mouse
        {
            audioManager.PlaySFX(audioManager.buttonClick); // Play click sound effect
            isDragging = true; // Start dragging
        }

        // Stop dragging when right mouse button is released
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false; // End dragging
        }

        // If dragging is active, handle camera rotation
        if (isDragging)
        {
            RotateCamera();
        }

        // Debug log for sprint input
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Left Shift key pressed
        {
            audioManager.PlaySFX(audioManager.keyDown); // Play key down sound effect
            isSprinting = true; // Set sprinting state to true
            UpdateSprintUI(true); // Update UI to show sprinting status
            animator.SetBool("IsSprinting", true); // Set the Animator's sprinting parameter to true
            Debug.Log("Sprint Started"); // Debug log for starting sprint
        }

        // Handle stopping sprinting input
        else if (Input.GetKeyUp(KeyCode.LeftShift)) // Left Shift key released
        {
            audioManager.PlaySFX(audioManager.keyDown); // Play key down sound effect
            isSprinting = false; // Set sprinting state to false
            UpdateSprintUI(false); // Update UI to show non-sprinting status
            animator.SetBool("IsSprinting", false); // Set the Animator's sprinting parameter to false
            Debug.Log("Sprint Stopped"); // Debug log for stopping sprint
        }

        // Update movement, shooting, and camera position
        Movement(); // Handle player movement
        Shooting(); // Handle shooting actions
        UpdateCameraPosition(); // Update camera position based on current mode

        // Update Animator with the current movement state
        UpdateAnimator(); // Update Animator based on the player's movement and actions
    }


    // Updates the Animator with the player's movement speed
    void UpdateAnimator()
    {
        // Calculate the speed based on the player's velocity in the x and z directions
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        // Set the Animator's "Speed" parameter to the calculated speed
        animator.SetFloat("Speed", speed);
    }

    // Handles the toggling of cursor lock and unlock
    void HandleCursorLockToggle()
    {
        // Check if the 'L' key is pressed to toggle cursor lock
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Play the sound effect for key press
            audioManager.PlaySFX(audioManager.keyDown);

            // Unlock the cursor first to ensure it is not locked
            UnlockCursor();

            // Toggle the cursor lock state
            isCursorLocked = !isCursorLocked;

            // Apply the appropriate cursor lock state
            if (isCursorLocked)
            {
                LockCursor(); // Lock the cursor if it was previously unlocked
            }
        }
    }


    // Handles switching between first-person and third-person camera modes
    void HandleCameraModeSwitch()
    {
        // Check if the 'V' key is pressed to switch camera modes
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            // Play the sound effect for key press
            audioManager.PlaySFX(audioManager.keyDown);

            // Switch between first and third person
            SwitchCameraMode();
        }
    }

    // Locks the cursor in the center of the screen and hides it
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Make the cursor invisible
    }

    // Unlocks the cursor, allowing it to move freely and be visible
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }


    // Rotates the camera based on mouse input
    void RotateCamera()
    {
        // Get horizontal and vertical mouse input, scaled by rotationSpeed
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Rotate the player object around the y-axis
        transform.Rotate(Vector3.up * horizontal);

        // Get the current rotation of the camera
        Vector3 currentRotation = Camera.main.transform.localEulerAngles;

        // Calculate the desired x rotation, adjusted by vertical mouse movement
        float desiredX = currentRotation.x - vertical;

        // Clamp the x rotation to prevent the camera from rotating too far up or down
        desiredX = Mathf.Clamp(desiredX, -80, 80);

        // Set the new rotation for the camera
        Camera.main.transform.localEulerAngles = new Vector3(desiredX, currentRotation.y, currentRotation.z);
    }

    // Switches between first-person and third-person camera modes
    void SwitchCameraMode()
    {
        // Toggle the camera mode between FirstPerson and ThirdPerson
        if (currentMode == CameraMode.FirstPerson)
        {
            currentMode = CameraMode.ThirdPerson;
        }

        else
        {
            currentMode = CameraMode.FirstPerson;
        }

        // Update the camera position based on the new mode
        UpdateCameraPosition();
    }

    // Updates the camera's active state based on the current camera mode
    void UpdateCameraPosition()
    {
        // Enable the appropriate camera based on the current mode
        if (currentMode == CameraMode.FirstPerson)
        {
            firstPersonCamera.gameObject.SetActive(true);
            thirdPersonCamera.gameObject.SetActive(false);
        }

        else
        {
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
        }
    }

    //forbuttons

    // Called when the button for switching camera views is pressed
    public void OnSwitchViewButtonPressed()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);
        // Switch the camera mode between FirstPerson and ThirdPerson
        SwitchCameraMode();
    }

    // Called when the button for toggling cursor lock is pressed
    public void OnToggleCursorLockButtonPressed()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);
        // Toggle the cursor lock state
        ToggleCursorLock();
    }

    // Called when the button for toggling sprint is pressed
    public void OnSprintButtonPressed()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);
        // Toggle sprint status
        isSprinting = !isSprinting;
        // Update the UI to reflect the sprint status
        UpdateSprintUI(isSprinting);
        // Update the Animator to reflect sprinting
        animator.SetBool("IsSprinting", isSprinting);
    }

    // Toggles the cursor lock state
    void ToggleCursorLock()
    {
        // Switch between locked and unlocked cursor states
        isCursorLocked = !isCursorLocked;
        if (isCursorLocked)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    void Movement()
    {
        // Get input from the horizontal and vertical axes (usually mapped to WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        // Apply sprint multiplier if sprinting
        if (isSprinting)
        {
            movement *= sprintMultiplier;
        }

        // Update the Rigidbody's velocity for movement. Preserve the current y-velocity to avoid affecting vertical movement.
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z) * movementSpeed;

        // Check if the player is moving based on horizontal or vertical input
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Handle footstep sounds and animation states while moving
            if (isSprinting)
            {
                // Set footstep sound to sprinting sound if not already set and play it
                if (audioSource.clip != sprintFootstepClip)
                {
                    audioSource.clip = sprintFootstepClip;
                    audioSource.Play();
                }

                // Update Animator parameters for sprinting
                animator.SetBool("IsSprinting", true);
                animator.SetBool("IsWalking", false);
            }

            else
            {
                // Set footstep sound to walking sound if not already set and play it
                if (audioSource.clip != normalFootstepClip)
                {
                    audioSource.clip = normalFootstepClip;
                    audioSource.Play();
                }

                // Update Animator parameters for walking
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsSprinting", false);
            }
        }

        else
        {
            // Stop the footstep sound when not moving
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Update Animator parameters to stop walking or sprinting animation
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsSprinting", false);
        }

        // Update Animator based on the speed
        UpdateAnimator();
    }

    void Shooting()
    {
        //extra prev for mouse input shooting
        /*
        if (Input.GetKeyDown(KeyCode.Mouse0) && gameManager.AmmoCount > 0)
        {
            PlayerGun.Instance.Shooting();
            rb.velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Mouse0) && gameManager.AmmoCount > 0)
        {
            PlayerGun.Instance.RapidShooting();
            rb.velocity = Vector3.zero;
        }
        */

        // Check if the Space key is pressed down and there is ammo available
        if (Input.GetKeyDown(KeyCode.Space) && gameManager.AmmoCount > 0)
        {
            // Play sound effect for shooting
            audioManager.PlaySFX(audioManager.keyDown);
            // Trigger normal shooting
            PlayerGun.Instance.Shooting();
            // Stop the player's movement
            rb.velocity = Vector3.zero;
        }

        // Check if the Space key is held down and there is ammo available
        if (Input.GetKey(KeyCode.Space) && gameManager.AmmoCount > 0)
        {
            // Play sound effect for shooting
            audioManager.PlaySFX(audioManager.keyDown);
            // Trigger rapid shooting
            PlayerGun.Instance.RapidShooting();
            // Stop the player's movement
            rb.velocity = Vector3.zero;
        }
    }


    // Triggered when the collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Call the TakeDamage method from GameManager to apply damage
            gameManager.TakeDamage(5);
        }
    }

    // Handles the shooting action
    public void HandleShooting()
    {
        // Check if ammo is available
        if (gameManager.AmmoCount > 0)
        {
            // Play the sound effect for shooting
            audioManager.PlaySFX(audioManager.buttonClick);
            PlayerGun.Instance.Shooting(); // Trigger shooting action
            rb.velocity = Vector3.zero; // Optional: Reset velocity if needed
        }

        else
        {
            // Log a message if there is no ammo available
            Debug.Log("No ammo available!");
        }
    }

    // Method to update the UI elements related to sprinting status
    void UpdateSprintUI(bool sprinting)
    {
        // If the sprint status text UI element is assigned
        if (sprintStatusText != null)
        {
            // Update the text to display "Sprinting" if sprinting, otherwise "Not Sprinting"
            sprintStatusText.text = sprinting ? "Sprinting" : "Not Sprinting";
        }

        // If the sprint icon UI element is assigned
        if (sprintIcon != null)
        {
            // Change the color of the sprint icon: green if sprinting, otherwise white
            sprintIcon.color = sprinting ? Color.green : Color.white; // Change color based on sprinting
        }
    }
}