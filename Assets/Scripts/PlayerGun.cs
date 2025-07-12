using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    Transform firingPoint; // Point from which the bullets will be fired

    [SerializeField]
    GameObject bulletPrefab; // Prefab of the bullet to be instantiated

    [SerializeField]
    float firingRate; // Rate at which bullets can be fired (in seconds)

    public static PlayerGun Instance; // Singleton instance of PlayerGun

    public float LastShot = 0; // Time when the last shot was fired
    public float delayTime = 2.0f; // Delay time for rapid shooting (not used in current methods)

    GameManager gameManager; // Reference to the GameManager for managing game state


    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Initialize the singleton instance
        Instance = GetComponent<PlayerGun>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager in the scene using its tag
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to handle shooting bullets
    public void Shooting()
    {
        // Check if the time elapsed since the last shot is greater than the firing rate
        //Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        if (LastShot + firingRate <= Time.time)
        {
            // Update the time of the last shot
            LastShot = Time.time;

            // Instantiate a bullet at the firing point with the firing point's rotation
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);

            // Decrement the ammo count in the GameManager
            gameManager.AmmoCount -= 1;
        }
    }

    // Method to handle rapid shooting bullets
    public void RapidShooting()
    {
        // Check if the time elapsed since the last shot is greater than the firing rate
        if (LastShot + firingRate <= Time.time)
        {
            
            firingRate = 0.1f; // Set the firing rate to a faster value for rapid shooting
            LastShot = Time.time; // Update the time of the last shot
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation); // Instantiate a bullet at the firing point with the firing point's rotation
            gameManager.AmmoCount -= 1; // Decrement the ammo count in the GameManager

        }
    }
}
