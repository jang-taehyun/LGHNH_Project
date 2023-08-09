using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // single ton //

    // member variable //
    static GameObject container;
    static DataManager inst;
    public string GameDataFileName = "GameData.json";
    public Data data = new Data();
    
    // interface //
    public static DataManager Inst
    {
        get
        {
            // if inst is null, create DataManager
            if (!inst)
            {
                container = new GameObject();
                container.name = "DataManager";
                inst = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);

                for (int i = 0; i < DataManager.inst.data.GetItemVariableNum(); i++)
                    DataManager.inst.data.CurrentHaveItem[i] = false;
            }

            return inst;
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + '/' + GameDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            Debug.Log("Success : Data load");
        }

        Debug.Log("load function Debug");
        DebugData();
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + '/' + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);

        Debug.Log("Save function Debug");
        DebugData();
    }

    public void DebugData()
    {
        Debug.Log("Check Data start");

        // CurrentHaveItemNum debug
        if (data.CurrentHaveItem.Length == 0)
            Debug.Log("There's no Item");
        else
            for (int i = 0; i < data.CurrentHaveItem.Length; i++)
                Debug.Log($"{i}'th item state : " + data.CurrentHaveItem[i]);

        Debug.Log("Check Data end");
    }
}
