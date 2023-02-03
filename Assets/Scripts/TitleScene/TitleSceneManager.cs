using Const;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public sealed class TitleSceneManager : SceneFrameBase
{
    #region SerializeField
    [SerializeField]
    private MainGUITitleSceneCanvasManager mainGUICanvasManager;
    [SerializeField]
    private TextAsset stageFile;
    #endregion


    protected override void Awake()
    {
        base.Awake();
    }

    // 起動処理
    protected override void Start()
    {


        base.PlayBGM();


        // ユーザデータ取得
        UserData userData = SaveManager.Instance.LoadSaveData<UserData>();
        if (userData == null)
        {
            // データが無い場合、ここで新規作成
            userData = new UserData();
            SaveManager.Instance.Save(userData);

        }

        DataTable stageData;
        if (SaveManager.Instance.DataSet.Tables[TableName.STAGE_TABLE] == null)
        {
            stageData = DataTableManager.LoadTextToDataTable(stageFile, TableName.STAGE_TABLE);
            SaveManager.Instance.DataSet.Tables.Add(stageData); // Datasetにマスタを保持
        }
        else
        {
            stageData = SaveManager.Instance.DataSet.Tables[TableName.STAGE_TABLE];
        }
       

        mainGUICanvasManager.Init(userData.HighScore, userData.StageNo, stageData);
        
    }


}
