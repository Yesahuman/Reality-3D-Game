using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    private Animator _doorAnimator; // Reference to the Animator component on the door


    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the same GameObject as this script
        _doorAnimator = GetComponent<Animator>();
    }


    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Trigger the door to open by setting the "Open" trigger in the Animator
            _doorAnimator.SetTrigger("Open");
        }
    }

    // This method is called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit(Collider other)
    {
        // Automatically close the door when the player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            // Trigger the door to close by setting the "Closed" trigger in the Animator
            _doorAnimator.SetTrigger("Closed");
        }
    }
}
