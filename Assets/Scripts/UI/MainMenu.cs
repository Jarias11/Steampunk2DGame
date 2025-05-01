using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("Main Town");
    }
    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game Quit (only works in build)");
    }
}
