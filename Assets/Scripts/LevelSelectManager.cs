using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        // Start the asynchronous loading process
        StartCoroutine(LoadLevelAsync(levelName));
    }

    // Coroutine for async loading with a loading screen
    private IEnumerator LoadLevelAsync(string levelName)
    {
        // Start the asynchronous scene load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        
        // Prevent the scene from immediately switching until fully loaded
        asyncLoad.allowSceneActivation = false;

        // While the scene is loading, update the progress bar
        while (!asyncLoad.isDone)
        {
            // asyncLoad.progress goes from 0 to 0.9, so multiply by 100 for a percentage
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Once progress reaches 0.9 (90%), the scene is essentially ready
            if (asyncLoad.progress >= 0.9f)
            {
                // Show the "Press any key" or delay option here if desired
                yield return new WaitForSeconds(0.5f); // Optional delay for smoother transition

                // Activate the new scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; // Continue the next frame
        }
    }
}
