using UnityEngine;
using UnityEngine.SceneManagement;

// Handles teleporting the player to a different scene when they enter a trigger collider
public class Teleporter : MonoBehaviour
{
    // The name of the scene to load when the player collides with the teleporter
    public string sceneName;

    // Called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger collider
        Debug.Log("Collision Detected with: " + other.name); // Log the name of the colliding object

        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Log a message indicating the player has entered the teleporter
            Debug.Log("Player entered the teleporter!"); // Log when the player enters the trigger

            // Load the specified scene
            SceneManager.LoadScene(sceneName);
        }
    }
}