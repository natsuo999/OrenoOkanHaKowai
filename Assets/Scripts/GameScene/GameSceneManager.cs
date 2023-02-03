using Const;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameSceneManager : SceneFrameBase
{
    #region SerializeField
    [SerializeField]
    private MainGUIGameSceneCanvasManager mainGUICanvasManager;
    [SerializeField]
    private TextAsset satageParamText;
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


        DataTable stageParamData;
        if (SaveManager.Instance.DataSet.Tables[TableName.STAGE_PARAM_TABLE] == null)
        {
            stageParamData = DataTableManager.LoadTextToDataTable(satageParamText, TableName.STAGE_PARAM_TABLE);
            SaveManager.Instance.DataSet.Tables.Add(stageParamData); // Datasetにマスタを保持
        }
        else
        {
            stageParamData = SaveManager.Instance.DataSet.Tables[TableName.STAGE_PARAM_TABLE];
        }

        mainGUICanvasManager.Initialize(userData, stageParamData);

    }

    protected override void OnDestroy()
    {
        SoundManager.Instance?.StopBGM();
    }
}
