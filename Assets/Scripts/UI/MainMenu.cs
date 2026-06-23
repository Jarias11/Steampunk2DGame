using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreenPanel;
    public Button newGameButton;
    public Button continueButton;

    [SerializeField] private GameObject noSavedGame = null;

    private string saveFilePath;

    private void Start()
    {
        // IMPORTANT:
        // Your SaveManager saves to save.json, not savefile.json
        saveFilePath = Application.persistentDataPath + "/save.json";

        continueButton.interactable = File.Exists(saveFilePath);

        if (noSavedGame != null)
            noSavedGame.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(StartNewGameRoutine());
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            if (noSavedGame != null)
                noSavedGame.SetActive(true);

            return;
        }

        StartCoroutine(ContinueGameRoutine());
    }

    private IEnumerator StartNewGameRoutine()
    {
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(true);

        // Load persistent manager scene
        AsyncOperation coreLoad = SceneManager.LoadSceneAsync("WorldCore", LoadSceneMode.Additive);
        yield return new WaitUntil(() => coreLoad.isDone);

        // Load starting gameplay scene
        AsyncOperation worldLoad = SceneManager.LoadSceneAsync("Farm", LoadSceneMode.Additive);
        yield return new WaitUntil(() => worldLoad.isDone);

        // Optional: make Farm the active scene
        Scene farmScene = SceneManager.GetSceneByName("Farm");
        if (farmScene.IsValid())
            SceneManager.SetActiveScene(farmScene);

        // Unload menu
        SceneManager.UnloadSceneAsync("Start Menu");
    }

    private IEnumerator ContinueGameRoutine()
    {
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(true);

        // Load persistent manager scene
        AsyncOperation coreLoad = SceneManager.LoadSceneAsync("WorldCore", LoadSceneMode.Additive);
        yield return new WaitUntil(() => coreLoad.isDone);

        // For now, load Farm because your current SaveData does not save scene name
        AsyncOperation worldLoad = SceneManager.LoadSceneAsync("Farm", LoadSceneMode.Additive);
        yield return new WaitUntil(() => worldLoad.isDone);

        Scene farmScene = SceneManager.GetSceneByName("Farm");
        if (farmScene.IsValid())
            SceneManager.SetActiveScene(farmScene);

        // Now that player/health objects exist, load saved data
        SaveManager.Instance.LoadGame(
            FindFirstObjectByType<PlayerMovement>(),
            FindFirstObjectByType<PlayerHealth>()
        );

        SceneManager.UnloadSceneAsync("Start Menu");
    }
}