using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called while this collider is colliding with another collider
    private void OnCollisionStay(Collision collision)
    {
        // Log a message whenever a collision is ongoing with another object
        Debug.Log("OnCollisionStay with " + collision.gameObject.name);
    }

    // Called when this collider stops colliding with another collider
    private void OnCollisionExit(Collision collision)
    {
        // Log a message whenever the collision ends with another object
        Debug.Log("OnCollisionExit with " + collision.gameObject.name);
    }


}

