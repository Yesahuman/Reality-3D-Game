using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandHereTrigger : MonoBehaviour
{
    public AudioClip standHereClip; // Audio to play on collision
    public Canvas standHereCanvas;  // World Space Canvas to display "Stand here"
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Get or add the AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the Canvas is active at the start
        standHereCanvas.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger is with the player (you can adjust the tag)
        if (other.CompareTag("Player"))
        {
            // Play the audio clip
            if (standHereClip != null)
            {
                audioSource.PlayOneShot(standHereClip);
            }

            // Deactivate the Canvas to hide the message when the player is within the trigger
            if (standHereCanvas != null)
            {
                standHereCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Stop the audio if it is playing
            audioSource.Stop();
        }

        // Reactivate the Canvas to show the message again when the player exits the trigger
        if (standHereCanvas != null)
        {
            standHereCanvas.gameObject.SetActive(true);
        }
    }
}
