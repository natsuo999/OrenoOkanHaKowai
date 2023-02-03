using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Const;
using System;
using System.Data;

public sealed class RegistScoreRanking : MonoBehaviour, IRankingScene
{
    #region SerializeField
    [SerializeField] GameObject rankInPanel;
    [SerializeField] GameObject rankOutPanel;
    [SerializeField] Text InputFieldText;
    #endregion

    #region 定数
    private const string serverAddRecordPhpFileName = "AddRecord.php";
    #endregion

    public IRankingScene.ErrorRankingScenenDelegate ErrorActionMethod { get; set; }
    public DataTable ErrorMessageData { get; set; }
    public delegate void CompleteRegistScoreDelegate();
    public CompleteRegistScoreDelegate CompleteMethod;

    #region Fields変数
    private WebSererRequestClient dbManager;
    private delegate void ErrorPanelButtonDelegate(string aaa);
    private ErrorPanelButtonDelegate ErrorPanelButtonDelegateMethod;
    private string inputTextStr;
    private string myScore;
    #endregion

    public void Initialize(WebSererRequestClient db, UserData userData)
    {
        dbManager = db;
        myScore = userData.Score.ToString();

    }

    #region ランキング登録処理
    /// <summary>
    /// ランキング登録ボタン押下時
    /// </summary>
    public void RegistScoreRunking(string inputText)
    {

        if (inputText == "")
        {
            inputText = "NoName";
        }

        // input文字を保持
        inputTextStr = inputText;

        // 入力可能文字数は最大12文字(InputFieldのCharacterLimitで設定)
        // else if (inputText.Length > 13) { // 入力不可 }; 

        InstanceStates.rankingScene = this;
        SaveManager.Instance.ProcessID = ServerConnectProcessID.POST_SCORE_DATA;

        // ランキング登録
        StartCoroutine(RegistScoreRunkingCoroutine(inputTextStr));

    }

    private IEnumerator RegistScoreRunkingCoroutine(string inputNameText)
    {

        // サーバにポストするデータを設定
        Dictionary<string, string> dic = new Dictionary<string, string>();
        // user_idはAutoincreamentで自動付番
        dic.Add("user_name", inputNameText);
        dic.Add("score", myScore);

        // スコア+名前から算出したハッシュ値をサーバに送信する
        dic.Add("hashValue", GetHash(inputNameText + myScore + "club_y"));

        // サーバ通信が完了するまで待機
        yield return StartCoroutine(dbManager.PostCall(serverAddRecordPhpFileName, dic, this.RegistRunkAction, this.ErrorAction));


    }

    private string GetHash(string input)
    {
        string hashValue = "";
        try
        {
            
            hashValue = GenerateHashValue.GenerateSHA256HashValue(input);

        }
        catch(Exception ex)
        {
            string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_HASH_GENERATE);
            ErrorAction(ServerConnectErrorID.ERROR_HASH_GENERATE, errorMessage + "\r\n" + ex.Message);
            Debug.Log(ex);
        }
        
        return hashValue;
    }

    // エラー時のアクションを定義
    private void ErrorAction(int errorID, string errorDetail)
    {

        ErrorActionMethod(errorID, errorDetail);

    }



    // ランキング登録時のアクションを定義
    private void RegistRunkAction(byte[] results)
    {
        if (results != null && results.Length > 0)
        {
            string res = System.Text.Encoding.GetEncoding("utf-8").GetString(results);

            // 登録失敗した場合
            // Panel初期化
            ResetPanel();

            // リトライボタン押下時の処理を設定
            ErrorPanelButtonDelegateMethod = RegistScoreRunking;

            //string errorMessage =  "AddRecordPhpErr:" + res;
            string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
            ErrorActionMethod(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage + "\r\n" +res);

        }
        else
        {
            // 登録成功した場合
            // Panel初期化
            ResetPanel();

            // 登録しましたダイアログ表示
            CompleteMethod();

        }
    }
    #endregion


    public void ResetPanel()
    {
        rankInPanel.SetActive(false);
        rankOutPanel.SetActive(false);

    }


    #region リトライボタン押下時
    public void OnRetryButton()
    {
        Debug.Log("RetryButton");

        // リトライボタン押下時の処理を実行
        ErrorPanelButtonDelegateMethod(inputTextStr);

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
}
