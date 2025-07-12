using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Students : MonoBehaviour
{
    public Canvas byStandersName; // The Canvas UI on top of the NPC
    public Canvas byStandersContainer; // Textbox Canvas UI
    public Transform player; // Reference to the player
    public float activationDistance = 5f; // Distance at which the Canvas will appear
    public AudioClip audioClip; // Audio clip to play when the player is near
    private AudioSource audioSource; // AudioSource component for playing audio
    private bool isAudioPlaying = false; // Flag to track if audio is currently playing

    void Start()
    {
        // Ensure the Canvas is disabled at the start
        byStandersName.gameObject.SetActive(false);
        byStandersContainer.gameObject.SetActive(false);

        // Get the AudioSource component or add one if it doesn't exist
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the audio clip to the AudioSource
        audioSource.clip = audioClip;

    }

    void Update()
    {
        // Calculate the distance between the player and the NPC
        float distance = Vector3.Distance(player.position, transform.position);

        // If the player is within the activation distance, enable the Canvas and play the audio
        if (distance <= activationDistance)
        {
            byStandersName.gameObject.SetActive(true);
            byStandersContainer.gameObject.SetActive(true);

            if (!isAudioPlaying)
            {
                audioSource.Play();
                isAudioPlaying = true;
            }
        }
        else
        {
            // If the player is outside the activation distance, disable the Canvas and stop the audio
            byStandersName.gameObject.SetActive(false);
            byStandersContainer.gameObject.SetActive(false);

            if (isAudioPlaying)
            {
                audioSource.Stop();
                isAudioPlaying = false;
            }
        }
    }
}
