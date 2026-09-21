using System.IO;
using UnityEngine;

public class DataService : MonoBehaviour {

    public SaveData Data;

    void Awake() {
        Data = LoadData();
    }

    SaveData LoadData() {
        string saveFilePath = Application.persistentDataPath + "/data.json";

        if (File.Exists(saveFilePath)) {
            string jsonText = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonText);
            return data;
        }
        return new SaveData();
    }

    void SaveData() {
        string jsonText = JsonUtility.ToJson(Data);
        string saveFilePath = Application.persistentDataPath + "/data.json";
        File.WriteAllText(saveFilePath, jsonText);
    }
}
