using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    public GameObject loadingScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this function to start loading a new scene
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

        // Begin to load the scene you specified
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating as soon as it is loaded
        operation.allowSceneActivation = false;

        // While the asynchronous operation to load the new scene is not yet complete, update the progress bar
        while (!operation.isDone)
        {
            // Check if the load has finished
            if (operation.progress >= 0.9f)
            {
                // Progress bar is full, you can activate the scene
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Deactivate the loading screen after the scene is fully loaded
        loadingScreen.SetActive(false);
    }
}
