using Const;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGUIRankingSceneCanvasManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private PanelRankInCanvas panelRankInCanvas;
    [SerializeField]
    private PanelOutOfRankCanvas panelOutOfRankCanvas;
    [SerializeField]
    private PanelRankingListCanvas panelRankingListCanvas;
    #endregion

    #region Fields変数

    private CheckScoreRanking chackScoreRanking;
    private RegistScoreRanking registScoreRanking;
    private GetRankDataList getRankDataList;

    private WebSererRequestClient dbManager;
    private UserData userData;
    private DataTable _errorMessageData;

    #endregion

    public void Init(WebSererRequestClient req, UserData data, DataTable errorMessageData)
    {
        dbManager = req;
        userData = data;
        _errorMessageData = errorMessageData;

        chackScoreRanking = this.GetComponent<CheckScoreRanking>();
        registScoreRanking = this.GetComponent<RegistScoreRanking>();
        getRankDataList = this.GetComponent<GetRankDataList>();

        // ランクシーンに関連する処理をまとめて初期設定
        IRankingScene[] rankingScenes = { chackScoreRanking, registScoreRanking, getRankDataList };
        foreach (var scene in rankingScenes)
        {
           
            // 各機能実行時に発生したエラー処理は
            // ここで定義した共通のダイアログを表示する
            scene.ErrorActionMethod += ErrorAction;
            scene.ErrorMessageData = errorMessageData;

            scene.Initialize(dbManager, userData);

        }

        SetPanelAction();

        // ランクチェック処理
        chackScoreRanking.ExecuteCheckRank();

    }

    #region privateメソッド
    private void SetPanelAction()
    {

        PanelRankSceneBase[] panelCanvas = { panelRankInCanvas, panelOutOfRankCanvas, panelRankingListCanvas };
        foreach (var canvas in panelCanvas)
        {
            // 戻るボタン押下時の処理を設定
            canvas.OnDialogCloseButtonMethod = OnDialogCloseButton;
        }


        // ランキング登録押下時の処理を設定
        panelRankInCanvas.RegistMethod += registScoreRanking.RegistScoreRunking;

        // ランキング登録成功時の処理を設定
        registScoreRanking.CompleteMethod += CompleteAction;


    }

    #region method

    private void ErrorAction(int errorID, string errorDetail="")
    {
        // todo ここでエラーIDからメッセージを取得したほうが効率がよいかも？
        // ⇒ネットワークエラーなどはエラー詳細が独自に出力されるので引数でそのまま渡して表示する
        if(errorDetail=="")
        {
            errorDetail = GetErrorMessage(_errorMessageData, errorID);
        }
        
        string errorMessage = "ErrorID:" + errorID + "\r\n" + "ErrorDetail:" + errorDetail;
        DialogManager.Instance.GenerateDialog(errorMessage,
                                              "キャンセル",
                                              "リトライ",
                                              //DialogManager.Instance.DestroyDialog,
                                              OnDialogCloseButton,
                                              OnErrorDialogRetryButton
                                             );

    }

    private void CompleteAction()
    {

        DialogManager.Instance.GenerateDialog("登録しました",
                                              "OK",
                                               OnConfirmDialogOKButton
                                             );

    }


    #endregion

    #region ConfirmDialogのOKボタン押下時
    /// <summary>
    /// OKボタン押下時の処理
    /// </summary>
    //
    private void OnConfirmDialogOKButton()
    {

        DialogManager.Instance.DestroyDialog();

        // ランク一覧取得処理
        getRankDataList.ExecuteGetRankData();


    }

    #endregion

    #region ErrorDialogのリトライボタン押下時
    /// <summary>
    /// リトライボタン押下時の処理
    /// </summary>
    private void OnErrorDialogRetryButton()
    {

        DialogManager.Instance.DestroyDialog();

        // ErrorDialogは共通だがリトライ押下時に
        // どの処理(クラス)が実行中かによってリトライ処理の内容を変更する
        InstanceStates.rankingScene.OnRetryButton();

    }

    #endregion

    #region 戻るボタン押下時
    /// <summary>
    /// 戻るボタン押下時
    /// </summary>
    private void OnDialogCloseButton()
    {
        // タイトルに戻る際にパラメータリセット
        SaveData(userData, 0, 1);

        SceneManager.LoadScene(SceneName.TITLE_SCENE);
    }

    public void SaveData(UserData data, int score, int stageNo)
    {
        // 次のステージへ進むため、スコアとステージNoを保持
        data.Score = score;
        data.StageNo = stageNo;
        SaveManager.Instance.Save(data);


    }
    #endregion

    public string GetErrorMessage(DataTable data, int errorId, string whereColumnName = "ErrorId", string selectColumnName = "ErrorMessage")
    {
        string errorMessage = DataTableManager.SelectStrDataFromDatatable(data
                                                    , whereColumnName
                                                    , errorId
                                                    , selectColumnName);

        return errorMessage;
    }

    #endregion
}
