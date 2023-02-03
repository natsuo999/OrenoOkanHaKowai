using Const;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを管理するクラスの基底クラス
/// </summary>
public class SceneFrameBase : MonoBehaviour
{
    [SerializeField] private string _secneName = null;
    public string SecneName
    {
        get
        {
            return _secneName;
        }
        set
        {
            _secneName = value;
        }
    }

    [SerializeField] private UserData _userData = null;
    public UserData UserData
    {
        get
        {
            return _userData;
        }
        set
        {
            _userData = value;
        }
    }

    // protected:派生クラスから呼び出されるメソッド
    // awakeはstartの前に呼び出される
    protected virtual void Awake()
    {
        // シーン名を設定
        SecneName = SceneManager.GetActiveScene().name;

    }

    protected virtual void Start()
    {


    }

    protected virtual void PlayBGM()
    {
        switch(SecneName)
        {
            case SceneName.TITLE_SCENE:
                SoundManager.Instance?.PlayBGM(SoundManager.BGM_Type.Title);
                break;
            case SceneName.GAME_SCENE:
                SoundManager.Instance?.PlayBGM(SoundManager.BGM_Type.Game);
                break;

        }
        
    }

    protected virtual DataTable GetDataTable(TextAsset satageParamText, string tableName)
    {
        DataTable stageParamData;
        if (SaveManager.Instance.DataSet.Tables[tableName] == null)
        {
            stageParamData = DataTableManager.LoadTextToDataTable(satageParamText, tableName);
            SaveManager.Instance.DataSet.Tables.Add(stageParamData); // Datasetにマスタを保持
        }
        else
        {
            stageParamData = SaveManager.Instance.DataSet.Tables[tableName];
        }

        return stageParamData;


    }

    // オブジェクトが破棄されるときに呼び出される
    protected virtual void OnDestroy()
    {
        SoundManager.Instance?.StopBGM();
    }

}
