using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kino; // Namespace for Kino's glitch effects

public class GlitchWhenNear : MonoBehaviour
{
    public DigitalGlitch GlitchEffect; // Reference to the DigitalGlitch component for digital glitch effects
    public float Intensity; //Intensity of the DigitalGlitch effect
    public AnalogGlitch Glitchy; // Reference to the AnalogGlitch component for analog-style glitch effects
    public float ColorDrift; // Intensity of the color drift effect in AnalogGlitch


    // Called when another collider stays within the trigger collider attached to this object
    private void OnTriggerStay(Collider collider)
    {
        // Check if the collider belongs to the player
        if (collider.tag == "Player")
        {
            // Calculate the distance between the player and this object
            Vector3 distanceVector = collider.transform.position - transform.position;
            //Debug.Log(distanceVector.magnitude);

            // Set the intensity of the digital glitch effect based on the distance
            GlitchEffect.intensity = Intensity / distanceVector.magnitude;
            // Set the color drift of the analog glitch effect based on the distance
            Glitchy.colorDrift = ColorDrift / distanceVector.magnitude;
        }
    }

    // Called when another collider exits the trigger collider attached to this object
    private void OnTriggerExit(Collider collider)
    {
        // Check if the collider belongs to the player
        if (collider.tag == "Player")
        {
            // Reset the intensity of the digital glitch effect to 0
            GlitchEffect.intensity = 0f;

            // Reset the color drift of the analog glitch effect to 0
            Glitchy.colorDrift = 0f;

            // Log a message indicating the player has exited the area
            Debug.Log("Player exited, glitch effects reset.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Ensure glitch effects start at 0 when the game begins
        GlitchEffect.intensity = 0f;
        Glitchy.colorDrift = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
