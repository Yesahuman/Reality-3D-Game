using UnityEngine;

public class GoalScript : MonoBehaviour
{
    // Called when this GameObject collides with another GameObject
    void OnCollisionEnter(Collision other)
    {
        // Check if the other GameObject involved in the collision has the tag "Player"
        if (other.gameObject.tag.Equals("Player"))
        {
            // Notify the GameManager that the goal has been collected
            GameManager.instance.GoalCollected();

            // Destroy this GameObject (the goal)
            Destroy(gameObject);
        }
    }
}
