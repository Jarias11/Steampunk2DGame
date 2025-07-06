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
    public void SwapButtonPositions()
    {
        // Move Continue above New Game
        continueButton.transform.SetSiblingIndex(0);
        newGameButton.transform.SetSiblingIndex(1);
    }
}





/* I HAD TO COMMENT THIS OUT BECAUSE IT WAS CAUSING ERRORS AND NEEDED TO CHANGE THE SCENE STUFF. JUST USE SAVE/LOADING FROM SAVEMANAGER.CS 
SORRRRYYY 

AND THIS IS THE PATH TO THE SAVE FILE: Application.persistentDataPath + "/save.json".  IF U WANNA GO TO IT IN THE FILE EXPLORER, JUST COPY AND PASTE C:\Users\Jarias\AppData\LocalLow\DefaultCompany\2DSteampunk Game\
EXCEPT REPLACE JARIAS WITH UR USERNAME FOR THE PC. AND FEEL FREE TO DELETE FILE IF U WANNA TEST THE SAVE/LOAD FUNCTIONALITY.
IF U HIT NEW GAME, ITLL LOAD THE FARM SCENE AND I HAVE IT SET TO SAVE GAME BY PRESSING P  AND LOAD GAME BY PRESSING O FOR NOW BUT IF U CAN LINK IT
TO THE UI BUTTONS THAT WOULD BE GREAT. U CAN SEE THE CODE FOR P AND O IN THE UPDATE FUNCTION OF SAVEMANAGER 

ALSO YOUR LOGIC IS PRETTY SIMILAR TO MY SAVE MANAGER BUT I NEEDED TO SAVE A WHOLE LOT MORE STUFF THAN JUST PLAYER SO IT GOT COMPLEX.


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
         */