using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
   
    public float speed; // Speed of the spawner (currently unused)
    public GameObject EnemyObject; // Prefab for the enemy to be spawned

    // UI Text elements to display spawned and destroyed enemy counts
    public TMP_Text SpawnedEnemyText;
    public TMP_Text DestroyedEnemyText;

    // Count of enemies spawned and cooldown for spawning new enemies
    public int count;
    public float spawnCooldown = 5f; // Time between enemy spawns
    private float spawnTimer;

    // List to keep track of currently active enemies
    private List<GameObject> activeEnemies = new List<GameObject>();

    private int destroyedEnemyCount = 0; // Counter for destroyed enemies

    public int maxEnemies = 5; // Maximum number of enemies to have in the scene at once
    public Transform[] spawnPoints; // Array of spawn points for enemy instantiation

    // Reference to the GameManager for game-related operations
    private GameManager gameManager;

    // Flag to control if enemies should be spawned
    private bool isSpawning = false; // To track if spawning is active


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the spawn timer
        spawnTimer = spawnCooldown;

        // Set the initial text for destroyed enemies if the text component is assigned
        if (DestroyedEnemyText != null)
        {
            DestroyedEnemyText.text = "Enemies Destroyed: " + destroyedEnemyCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only spawn enemies if spawning is active
        if (isSpawning)
        {
            spawnTimer -= Time.deltaTime;

            // Spawn enemies if the number of active enemies is less than the maximum
            if (spawnTimer <= 0f && activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
                spawnTimer = spawnCooldown; // Reset the timer
            }
        }

        // Reference the GameManager
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is null in Spawner script!");
        }
    }

    // Method called when an enemy is destroyed
    public void OnEnemyDestroyed(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            Debug.Log("OnEnemyDestroyed called for: " + enemy.name);
            activeEnemies.Remove(enemy); // Remove the destroyed enemy from the list

            // Increment the destroyed enemy count
            destroyedEnemyCount++;

            // Update the UI text for destroyed enemies if assigned
            if (DestroyedEnemyText != null)
            {
                DestroyedEnemyText.text = "Enemies Destroyed: " + destroyedEnemyCount;
            }

            /*
            if (destroyedEnemyCount >= 10)
            {
                //SceneManager.LoadScene("End");
                gameManager.ShowWinPanel();
            }
            */
        }
    }

    // Method to instantiate a new enemy at a random spawn point
    void SpawnEnemy()
    {
        if (activeEnemies.Count < maxEnemies) // Only spawn if the number of active enemies is less than the limit
        {
            // Select a random spawn point from the available points
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 1f, 0);
            GameObject newEnemy = Instantiate(EnemyObject, spawnPosition, spawnPoint.rotation);
            newEnemy.tag = "Enemy";

            activeEnemies.Add(newEnemy); // Add the new enemy to the list

            count++;
            SpawnedEnemyText.text = "Enemies Spawned: " + count;

            Debug.Log("Spawned new Enemy at position: " + spawnPosition);
        }
    }

    // Method to start spawning enemies
    public void StartSpawning()
    {
        isSpawning = true;
    }

    // Method to stop spawning enemies
    public void StopSpawning()
    {
        isSpawning = false;
    }

}
