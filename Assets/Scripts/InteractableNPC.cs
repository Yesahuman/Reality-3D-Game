using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractableNPC : MonoBehaviour
{
    public GameObject Testingpanel; // Reference to the test panel
    public GameObject guiPanel; // Reference to the GUI panel
    public TMP_Text interactPromptText; // Reference to the TextMeshPro text for the prompt
    private bool isPlayerNearby = false; // To track if the player is nearby
    private bool isGuiVisible = false; // To track the visibility of the GUI

    public TMP_Text dialogueText; // Reference to the TextMeshPro text for the dialogue

    public Button interactButton; // Reference to the interact Button

    public Button nextButton; // Reference to the "Next" button
    public Button exitButton; // Reference to the Exit button

    public string[] dialogueLines; // Array to hold the lines of dialogue
    public AudioClip[] dialogueAudioClips; // Array to hold the audio clips for each line of dialogue
    private int currentLineIndex = 0; // To keep track of the current line of dialogue

    AudioManager audioManager; // Reference to the audiomanager
    private AudioSource audioSource; //ltr for clips
    PlayerMovement playerMovement; // Reference to the PlayerMovement script

    public Loading loadingScreenManager; // Reference to the Loading script


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the user interface elements
        InitializeUI();

        // Attempt to find the AudioManager instance in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            // Log an error if the AudioManager is not found
            Debug.LogError("AudioManager not found!");
        }

        // Attempt to get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If no AudioSource is found, add one to the GameObject
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Attempt to find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            // Log an error if the PlayerMovement script is not found
            Debug.LogError("PlayerMovement script not found!");
        }
    }

    private void InitializeUI()
    {
        // Check if interactPromptText is not null
        if (interactPromptText != null)
        {
            // Deactivate the interactPromptText UI element
            interactPromptText.gameObject.SetActive(false);
        }

        // Check if guiPanel is not null
        if (guiPanel != null)
        {
            // Deactivate the guiPanel UI element
            guiPanel.SetActive(false);
        }

        // Check if interactButton is not null
        if (interactButton != null)
        {
            // Deactivate the interactButton UI element
            interactButton.gameObject.SetActive(false);

            // Add a listener to handle button clicks
            interactButton.onClick.AddListener(OnInteractButtonClicked);
        }

        // Check if nextButton is not null
        if (nextButton != null)
        {
            // Deactivate the nextButton UI element
            nextButton.gameObject.SetActive(false);

            // Add a listener to handle button clicks
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        // Check if exitButton is not null
        if (exitButton != null)
        {
            // Deactivate the exitButton UI element
            exitButton.gameObject.SetActive(false);

            // Add a listener to handle button clicks
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Check if the player is nearby and the 'E' key is pressed
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isGuiVisible)
            {
                // If GUI is not visible, show the GUI and start the dialogue
                audioManager.PlaySFX(audioManager.keyDown); // Play a sound effect
                HideElements(); // Hide specific UI elements
                ToggleGUI(); // Show the GUI
                LockPlayerMovement(true); // Lock player movement
                Time.timeScale = 0f; // Pause the game
            }

            else
            {
                // If GUI is visible, hide the GUI and return to showing elements
                audioManager.PlaySFX(audioManager.keyDown); // Play a sound effect
                ShowElements(); // Show the previously hidden UI elements
                ToggleGUI(); // Hide the GUI
                LockPlayerMovement(false); // Unlock player movement
                Time.timeScale = 1f; // Resume the game
            }
        }


        // Check for keyboard inputs for dialogue controls when GUI is visible
        if (isGuiVisible)
        {
            // Handle showing the next dialogue in the sequence
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                audioManager.PlaySFX(audioManager.keyDown); // Play a sound effect
                ShowNextDialogue(); // Display the next dialogue
                Time.timeScale = 0f; // Pause the game
                LockPlayerMovement(true); // Lock player movement
                //OnNextButtonClicked();
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                // Handle exiting the dialogue and showing the previous elements
                audioManager.PlaySFX(audioManager.keyDown); // Play a sound effect
                ShowElements(); // Show the previously hidden UI elements
                ToggleGUI(); // Hide the GUI
                Time.timeScale = 1f; // Resumes the game if it was paused
                //OnExitButtonClicked();
                LockPlayerMovement(false); // Unlock player movement
            }

            // Check if the skip key is pressed
            if (Input.GetKeyDown(KeyCode.Q))
            {
                audioManager.PlaySFX(audioManager.keyDown); // Play a sound effect
                SceneManager.LoadScene("InGame"); // Load the "InGame" scene
            }
        }
    }

    void LockPlayerMovement(bool lockMovement)
    {
        if (playerMovement != null)
        {
            // If lockMovement is true, disable movement; otherwise, enable it
            playerMovement.enabled = !lockMovement;
        }
    }

    void HideElements()
    {
        // Hide UI elements related to the testing panel, prompt text, and interact button

        if (Testingpanel != null)
        {
            // Disable the Testingpanel game object if it exists
            Testingpanel.SetActive(false);
        }

        if (interactPromptText != null)
        {
            // Hide the interact prompt text if it exists
            interactPromptText.gameObject.SetActive(false);
        }

        if (interactButton != null)
        {
            // Hide the interact button if it exists
            interactButton.gameObject.SetActive(false);
        }
    }

    void ShowElements()
    {
        // Show UI elements related to the testing panel, prompt text, and interact button

        if (Testingpanel != null)
        {
            // Enable the Testingpanel game object if it exists
            Testingpanel.SetActive(true);
        }

        if (interactPromptText != null)
        {
            // Show the interact prompt text if it exists
            interactPromptText.gameObject.SetActive(true);
        }

        if (interactButton != null)
        {
            // Show the interact button if it exists
            interactButton.gameObject.SetActive(true);
        }
    }

    void ToggleGUI()
    {
        isGuiVisible = !isGuiVisible; // Toggle the visibility state
        guiPanel.SetActive(isGuiVisible); // Set the active state of the GUI panel

        if (isGuiVisible)
        {
            // Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;

            // Reset dialogue to the first line
            currentLineIndex = 0; 
            ShowNextDialogue();
        }

        else
        {
            // If the GUI is not visible, hide the next and exit buttons
            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(false); // Hide the next button when GUI is hidden
            }

            if (exitButton != null)
            {
                exitButton.gameObject.SetActive(false); // Hide the exit button when GUI is hidden
            }
        }
    }


    void ShowNextDialogue()
    {
        // Check if there are more lines of dialogue to show
        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex]; // Show the current line of dialogue

            // Play the corresponding audio clip
            if (dialogueAudioClips != null && dialogueAudioClips.Length > currentLineIndex)
            {
                audioSource.clip = dialogueAudioClips[currentLineIndex];
                audioSource.Play();
            }

            // Determine the button text based on the current line
            string buttonText = "Next"; // Default text


            // Check which line we're on and respond accordingly
            switch (currentLineIndex)
            {
                case 0:
                    // First line actions
                    Debug.Log("First line of dialogue.");
                    buttonText = "Hey...are you okay?"; // Set button text for the first line
                    break;

                case 1:
                    // Second line actions
                    Debug.Log("Second line of dialogue.");
                    buttonText = "I care because you're my friend."; // Set button text for the second line
                    break;

                case 2:
                    // Third line actions
                    Debug.Log("Third line of dialogue.");
                    buttonText = "I can't just leave you at this state.."; // Set button text for the third line
                    break;

                case 3:
                    // Fourth line actions
                    Debug.Log("Fourth line of dialogue.");
                    buttonText = "We are friends, friends help each other don't they?"; // Set button text for the fourth line
                    break;

                case 4:
                    // Firth line actions
                    Debug.Log("Fifth line of dialogue.");
                    buttonText = "I'll try my best to help you, please accept it."; // Set button text for the fifth line
                    break;

                case 5:
                    // Sixth line actions
                    Debug.Log("sixth line of dialogue");
                    buttonText = "You are worthy of my time."; // Set button text for sixth line
                    break;

                case 6:
                    // Seventh line actions
                    Debug.Log("seventh line of dialogue");
                    buttonText = "You're somebody."; // Set button text for xseventh line
                    break;

                // Add more cases as needed for additional dialogue lines

                default:
                    Debug.Log("Default action for any other line.");
                    buttonText = "Next"; // Default text
                    break;
            }

            // Change the button text for the last line
            if (currentLineIndex == dialogueLines.Length - 1)
            {
                buttonText = "Finish"; // Change button text to "Finish"
                Time.timeScale = 1f; // Resume the game time before changing the scene

                // Hide elements and the GUI panel
                HideElements();
                guiPanel.SetActive(false);
                Testingpanel.SetActive(false);

                Debug.Log("loading");
                // Load the next scene
                loadingScreenManager.Loadscene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            // Update the button text and make buttons visible
            nextButton.GetComponentInChildren<TMP_Text>().text = buttonText; // Apply the determined button text
            nextButton.gameObject.SetActive(true); // Show the next button
            exitButton.gameObject.SetActive(true); // Show the exit button

            // Move to the next line of dialogue
            currentLineIndex++;
        }

        else
        {
            // No more dialogue lines, hide the next button and show elements
            nextButton.gameObject.SetActive(false);
            ShowElements();
            ToggleGUI();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // Set flag indicating the player is near

            // Show the interaction prompt and button if they are not null
            if (interactPromptText != null)
            {
                interactPromptText.gameObject.SetActive(true);
            }

            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player has exited the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false; // Set flag indicating the player is no longer near

            // Hide the interaction prompt and button if they are not null
            if (interactPromptText != null)
            {
                interactPromptText.gameObject.SetActive(false);
            }

            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(false);
            }

            // If the GUI is visible, toggle it off
            if (isGuiVisible)
            {
                ToggleGUI();
            }
        }
    }

    void OnInteractButtonClicked()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Hide elements and toggle the GUI
        HideElements();
        ToggleGUI();

        // Lock player movement during interaction
        LockPlayerMovement(true);
    }

    void OnNextButtonClicked()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Show the next line of dialogue and stop the game
        ShowNextDialogue();
        Time.timeScale = 0f; // pause the game
    }

    void OnExitButtonClicked()
    {
        // Play button click sound effect
        audioManager.PlaySFX(audioManager.buttonClick);

        // Show previously hidden elements, toggle the GUI, and resume the game
        ShowElements();
        ToggleGUI();
        Time.timeScale = 1f; // Resumes the game if it was paused
        LockPlayerMovement(false); // Unlock player movement
    }
}
