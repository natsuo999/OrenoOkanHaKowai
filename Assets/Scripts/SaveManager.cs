using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public string ProcessID { get; set; } = "";
    public DataSet DataSet { get; set; } = new DataSet();

    private void Awake()
    {
        Instance = this;
    }

    // ロード
    public T LoadSaveData<T>()
    {
        // Json文字列をクラスに復元
        string json = PlayerPrefs.GetString("SAVE_DATA");
        T data = JsonUtility.FromJson<T>(json);

        return data;

    }

    // セーブ
    public void Save<T>(T data)
    {
        // SaveDataをJson化して保存
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SAVE_DATA", json);
    }
   
}
