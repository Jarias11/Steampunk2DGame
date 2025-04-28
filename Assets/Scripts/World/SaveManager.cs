using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public float[] playerPosition;
    public int playerHealth;
    public GameTimeData gameTimeData;
}


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            savePath = Application.persistentDataPath + "/save.json";
        }
        else Destroy(gameObject);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SaveManager.Instance.SaveGame(FindFirstObjectByType<PlayerMovement>(), FindFirstObjectByType<PlayerHealth>());

        if (Input.GetKeyDown(KeyCode.O))
            SaveManager.Instance.LoadGame(FindFirstObjectByType<PlayerMovement>(), FindFirstObjectByType<PlayerHealth>());
    }
    public void SaveGame(PlayerMovement pm, PlayerHealth ph)
    {
        SaveData data = new SaveData();
        Vector2 pos = pm.transform.position;
        data.playerPosition = new float[] { pos.x, pos.y, };
        data.playerHealth = ph.GetCurrentHealth();
        data.gameTimeData = GameTime.Instance.GetTimeData();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved to: " + savePath);
    }
    public void LoadGame(PlayerMovement playerMovement, PlayerHealth playerHealth)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Vector2 pos = new Vector2(data.playerPosition[0], data.playerPosition[1]);
        playerMovement.transform.position = pos;
        playerHealth.SetCurrentHealth(data.playerHealth);
        GameTime.Instance.SetTimeData(data.gameTimeData);

        Debug.Log("Game Loaded.");
    }





}


