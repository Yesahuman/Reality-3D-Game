using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Stores the initial position where the bullet was fired
    private Vector3 firingPoint;

    [SerializeField]
    private float bulletSpeed; // Speed of the bullet

    [SerializeField]
    private float maxBulletDistance; //Maximum distance the bullet can travel before being destroyed

    public int damage = 10; // Damage the bullet will inflict on the enemy


    // Start is called before the first frame update
    void Start()
    {
        // Record the initial firing position of the bullet
        firingPoint = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        // Handle bullet movement
        MoveBullet();
    }

    // Moves the bullet forward and checks if it has exceeded the maximum travel distance
    void MoveBullet()
    {
        // If the bullet has traveled beyond the maximum distance, destroy it
        if (Vector3.Distance(firingPoint, transform.position) > maxBulletDistance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Move the bullet forward based on its speed
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }


    // Detect collisions with other objects
    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet collides with an object tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Get the EnemyHealth component from the enemy object
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Inflict damage on the enemy
                enemyHealth.TakeDamage(damage);

                // Destroy the bullet after hitting the enemy
                Destroy(this.gameObject);
            }
        }
    }
}
