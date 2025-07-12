using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    // Reference to the loading screen GameObject
    public GameObject loadingScreen;

    // Reference to the UI Image component for the loading bar fill
    public Image loadingBarFill;

    // Public method to start loading a scene by its index
    public void Loadscene(int sceneId)
    {
        // Start the coroutine to load the scene asynchronously
        StartCoroutine(LoadsceneAsync(sceneId));
    }

    // Coroutine to load a scene asynchronously and update the loading screen
    IEnumerator LoadsceneAsync(int sceneid)
    {
        // Display the loading screen
        loadingScreen.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneid);

        // Define the minimum time the loading screen should be visible
        float minimumLoadingTime = 1f;

        // Track the elapsed time since the loading screen was shown
        float elapsedTime = 0f;


        // Continue updating the loading screen while the scene is loading or the minimum time has not passed
        while (!operation.isDone || elapsedTime < minimumLoadingTime)
        {
            // Calculate the progress of the loading operation
            // The progress value is clamped between 0 and 1
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the fill amount of the loading bar
            loadingBarFill.fillAmount = progressValue;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
            // Wait until the next frame
            yield return null;
        }

        // Hide the loading screen after loading is complete and the minimum time has passed
        loadingScreen.SetActive(false);
    }

}
