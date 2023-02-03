
using Const;
using System.Data;
using UnityEngine;


/// <summary>
/// ランキングシーンを管理するクラス
/// </summary>
public sealed class RankingSceneManager : SceneFrameBase
{

    [SerializeField]
    private WebSererRequestClient dbManager;
    [SerializeField]
    private MainGUIRankingSceneCanvasManager mainGUIRankingSceneCanvasManager;
    [SerializeField]
    private TextAsset errorMessageFile;

    protected override void Awake()
    {
        base.Awake();
    }

    // 起動処理
    protected override void Start()
    {
        Debug.Log(base.SecneName);

        // ユーザデータ取得
        UserData userData = SaveManager.Instance.LoadSaveData<UserData>();
        if (userData == null)
        {
            // データが無い場合、ここで新規作成
            userData = new UserData();
            SaveManager.Instance.Save(userData);

        }

        DataTable errorMessageData;
        if (SaveManager.Instance.DataSet.Tables[TableName.ERROR_MESSAGE_TABLE] == null)
        {
            errorMessageData = DataTableManager.LoadTextToDataTable(errorMessageFile, TableName.ERROR_MESSAGE_TABLE);
            SaveManager.Instance.DataSet.Tables.Add(errorMessageData); // Datasetにマスタを保持
        }
        else
        {
            errorMessageData = SaveManager.Instance.DataSet.Tables[TableName.ERROR_MESSAGE_TABLE];
        }

        mainGUIRankingSceneCanvasManager.Init(dbManager, userData, errorMessageData);

    }


}
