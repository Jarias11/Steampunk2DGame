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
        CreateSaveFile(); // Create dummy save data
        SceneManager.LoadScene("Main Town");
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
}
