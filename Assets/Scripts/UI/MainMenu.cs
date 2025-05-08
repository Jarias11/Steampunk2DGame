using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour {
    public string levelToLoad;
    public GameObject loadingScreenPanel;
    //public Slider loadingBar;
    public RectTransform newGameTransform;
    public RectTransform continueTransform;
    public Button newGameButton;
    public Button continueButton;
    private string saveFilePath;


    [SerializeField] private GameObject noSavedGame = null;
    public void StartGame() {
        CreateSaveFile(); // Create dummy save data
        SceneManager.LoadScene("Main Town");
        //StartCoroutine(LoadSceneAsync("Main Town"));
    }

    public void LoadGame(){
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            Debug.Log("Continuing from scene: " + data.savedScene);
            SceneManager.LoadScene(data.savedScene); // Load the saved scene
        }
        else
        {
            noSavedGame.SetActive(true); // Show error UI if no save exists
        }
    }
    public void Start()
    {
        saveFilePath = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(saveFilePath))
        {
            // Enable Continue
            continueButton.interactable = true;

            // Swap positions if desired
            SwapButtonPositions();
        }
        else
        {
            // Disable Continue button
            continueButton.interactable = false;
        }
    }

    public void CreateSaveFile(){
        GameData data = new GameData();
        data.playerName = "Player1";
        data.level = 1;
        data.positionX = 0;
        data.positionY = 0;
        data.positionZ = 0;
        data.savedScene = "Main Town"; //Save 

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savefile.json";

        File.WriteAllText(path, json);
        Debug.Log("Save file created at: " + path);
        }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game Quit (only works in build)");
    }
    public void SwapButtonPositions()
    {
        // Move Continue above New Game
        continueButton.transform.SetSiblingIndex(0);
        newGameButton.transform.SetSiblingIndex(1);
    }
}
