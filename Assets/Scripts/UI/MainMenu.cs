using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class MainMenu : MonoBehaviour {
    public string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;
    public void StartGame() {
        StartCoroutine(LoadGameSequence());
    }

    private IEnumerator LoadGameSequence() {
        // Step 1: Load WorldCore (persistent systems)
        AsyncOperation coreLoad = SceneManager.LoadSceneAsync("WorldCore", LoadSceneMode.Additive);
        yield return new WaitUntil(() => coreLoad.isDone);

        // Step 2: Load the starting map (FarmScene)
        AsyncOperation worldLoad = SceneManager.LoadSceneAsync("Farm", LoadSceneMode.Additive);
        yield return new WaitUntil(() => worldLoad.isDone);

        // Step 3: Unload the Start Menu scene (the one currently running)
        SceneManager.UnloadSceneAsync("Start Menu");
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game Quit (only works in build)");
    }
}