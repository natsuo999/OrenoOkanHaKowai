using Const;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public sealed class CheckScoreRanking: MonoBehaviour, IRankingScene
{
    #region SerializeField
    [SerializeField] GameObject rankInPanel;
    [SerializeField] GameObject rankOutPanel;

    [SerializeField] public Text RankInText;
    [SerializeField] public Text RankOutext;
    #endregion

    #region public delegate
    // エラー時の処理を設定
    //public delegate void Action(string errorId, string errorDetail);
    public IRankingScene.ErrorRankingScenenDelegate ErrorActionMethod { get; set; }
    public DataTable ErrorMessageData { get; set; }


    #endregion

    #region Fields変数

    // ランキング初期値
    private int rank = -1;

    // リトライ押下時の処理を設定
    // ボタンのデリゲートを場合によって変更する必要があるため定義
    private delegate void ErrorPanelButtonDelegate();
    private ErrorPanelButtonDelegate ErrorPanelButtonDelegateMethod;

    private WebSererRequestClient webRequest;

    private string myScore;
    private bool isBestScore;
    #endregion

    #region 定数
    private readonly string serverCheckRankInAdressPhpFileName = "CheckRankIn.php";
    #endregion

    public void Initialize(WebSererRequestClient req, UserData userData)
    {
        webRequest = req;
        webRequest.ErrorMessageData = ErrorMessageData;

        myScore = userData.Score.ToString();
        // test 
        //myScore = "9000";
        isBestScore = userData.IsMyBsetScore;

        // エラー画面のリトライ押下時処理を初期化
        ErrorPanelButtonDelegateMethod = null;



    }

    public void ExecuteCheckRank()
    {

        InstanceStates.rankingScene = this;
        SaveManager.Instance.ProcessID = ServerConnectProcessID.CHECK_RANK_IN;

        // Panel初期化
        ResetPanel();

        // ランキングチェック
        StartCoroutine(CheckRankingCoroutine());

    }

    #region ランキングチェック処理

    /// <summary>
    /// ランキングチェック処理
    /// </summary>
    private IEnumerator CheckRankingCoroutine()
    {

        // サーバ通信が完了するまで待機
        yield return StartCoroutine(webRequest.WebGetRequestCoroutine());
        

        if (!webRequest.IsDBServerConnect)
        {

            // エラー画面のリトライ押下時処理を設定
            ErrorPanelButtonDelegateMethod = ExecuteCheckRank;

            string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_NETWORK_REGIST_RANK);
            ErrorActionMethod(ServerConnectErrorID.ERROR_NETWORK_REGIST_RANK, errorMessage);

            // 処理終了
            yield break;
        }

        // サーバにポストするデータを設定
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("score", myScore);

        // サーバ通信が完了するまで待機
        // delegate関数でサーバ接続後の処理を指定
        yield return StartCoroutine(webRequest.PostCall(serverCheckRankInAdressPhpFileName, dic, this.CheckRankAction, this.ErrorAction));
       

        if (!webRequest.IsServerProcess)
        {
            // サーバ処理失敗
            // 処理終了
            ErrorPanelButtonDelegateMethod = ExecuteCheckRank;
            yield break;
        }


    }

    private void ErrorAction(int errorID, string errorDetail)
    {

        ErrorActionMethod(errorID, errorDetail);
    }



    // ランキングチェック時のアクションを定義
    private void CheckRankAction(byte[] results)
    {
        
        if (results.Length > 0)
        {
            
            string res = System.Text.Encoding.GetEncoding("utf-8").GetString(results);
            int result = 0;
            bool isConvert = int.TryParse(res, out result);
            if (!isConvert || result == 0)
            {

                string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
                ErrorActionMethod(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage);

            }
            else
            {
                string mybestText = "";
                if(isBestScore)
                {
                    mybestText = "  自己ベスト更新!!";
                }

                // -1(ランキング以外)もしくは取得したランキング
                rank = result;

                // パラメータランキング内の場合
                if (rank > 0)
                {

                    string message = myScore + "点　" + rank + "位でした!!" + mybestText;
                    ShowResultRankingPanel(rankInPanel, RankInText, message);

                }
                else
                {
                    string message = myScore + "点" + mybestText;
                    ShowResultRankingPanel(rankOutPanel, RankOutext, message);

                }
            }

        }
    }
    #endregion

    private void ShowResultRankingPanel(GameObject panelObject, Text panelObjectText, string message)
    {

        panelObject.SetActive(true);
        panelObjectText.text = message;
    }


    private void ResetPanel()
    {
        rankInPanel.SetActive(false);
        rankOutPanel.SetActive(false);
    
    }

    #region リトライボタン押下時
    public void OnRetryButton()
    {

        // 設定したリトライ処理を実行
        ErrorPanelButtonDelegateMethod();
    }

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
