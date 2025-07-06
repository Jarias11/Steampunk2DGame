using UnityEngine;

public class UICore : MonoBehaviour
{
    private static bool exists = false;

    void Awake()
    {
        if (exists)
        {
            Destroy(gameObject); // Prevent duplicates when reloading scenes during testing
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            exists = true;
        }
    }
}