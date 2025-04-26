using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public SaveContainer saveData;

    string saveLocation;
    public UnityEvent OnLoad;

    void Awake()
    {
        // Set up singleton
        if (instance != null)
        {
            Destroy(this);
            return;
        }   

        instance = this;

        // Set up data path
        saveLocation = Application.persistentDataPath + "/save.json";
    }

    void Start() {
        LoadData();
    }

    public void SaveData() {
        if (saveData == null) {
            saveData = new();
        }

        string jsonSaveData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveLocation, jsonSaveData);
    }

    public void LoadData() {

        // Create the default save file if not already exists
        if (!File.Exists(saveLocation))
        {
            SaveData();
            return;
        }

        // Convert file to class
        string savedString = File.ReadAllText(saveLocation);
        JsonUtility.FromJsonOverwrite(savedString, saveData);

        // Update saved
        OnLoad?.Invoke();
    }
}
