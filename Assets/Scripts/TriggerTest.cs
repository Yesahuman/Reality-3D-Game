using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter with " + other.gameObject.name);
    }

    // This method is called when another collider stays within the trigger collider attached to this object
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay with " + other.gameObject.name);
    }

    // This method is called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit with " + other.gameObject.name);
    }

}
