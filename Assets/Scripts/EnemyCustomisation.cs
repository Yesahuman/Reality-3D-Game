using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCustomisation : MonoBehaviour
{
    public Color[] colors; // Array of colors
    public string[] words; // Array of words
    public TMP_Text enemyLabel; // Reference to the UI Text component

    private Renderer enemyRenderer; // Reference to the Renderer component

    private Dictionary<string, Color> wordColorMapping; // Dictionary to map words to colors


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Renderer component
        enemyRenderer = GetComponent<Renderer>();

        // Initialize the dictionary and map words to colors
        wordColorMapping = new Dictionary<string, Color>();

        // Ensure there are enough colors for the words
        if (words.Length != colors.Length)
        {
            Debug.LogError("Number of words and colors must be the same!");
            return; // Exit the method if the lengths do not match
        }

        // Populate the dictionary with words as keys and colors as values
        for (int i = 0; i < words.Length; i++)
        {
            wordColorMapping[words[i]] = colors[i];
        }

        // Randomly select a word from the array
        string randomWord = words[Random.Range(0, words.Length)];

        // Set the color based on the selected word
        Color color;
        if (wordColorMapping.TryGetValue(randomWord, out color))
        {
            // Set the enemy's material color based on the selected word
            enemyRenderer.material.color = color;
        }
        else
        {
            // Log an error if the color mapping is not found
            Debug.LogError("Color not found for the selected word.");
        }

        // Set the label text for the enemy with the selected word
        if (enemyLabel != null)
        {
            enemyLabel.text = randomWord;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
