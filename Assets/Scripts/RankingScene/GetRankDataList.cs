using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Data;
using Const;

public class GetRankDataList :MonoBehaviour, IRankingScene
{
    [SerializeField] GameObject rankingListPanel;
    [SerializeField] public Text[] RankTopText;

    #region Fields変数
    private RankDataParam[] RankData;
    private delegate void ErrorPanelButtonDelegate();
    private ErrorPanelButtonDelegate ErrorPanelButtonDelegateMethod;
    private WebSererRequestClient webRequest;
    private string _uri = "GetRankingData.php";
    #endregion

    #region public delegate
    // エラー時の処理を設定
    public IRankingScene.ErrorRankingScenenDelegate ErrorActionMethod { get; set; }
    public DataTable ErrorMessageData { get; set; }

    public int Score { get; set; }

    #endregion


    [Serializable]
    public class RankDataParam
    {
        public string user_name;
        public int score;
    }

    public void Initialize(WebSererRequestClient db, UserData userData)
    {
        webRequest = db;

        // userData使用しない
    }

    public void ExecuteGetRankData()
    {
        InstanceStates.rankingScene = this;

        rankingListPanel.SetActive(true);

        // ランクテキストをクリア
        int cnt = 1;
        foreach (var rankText in RankTopText)
        {
            rankText.text = cnt + "位" + " " + "-" ;
            cnt++;
        }

        // ランク取得
        StartCoroutine(StartProcessCoroutine());

    }

    #region ランクデータ取得

    /// <summary>
    /// ランクデータ取得
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartProcessCoroutine()
    {
   
        yield return StartCoroutine(webRequest.GetCall(_uri, LoadRankDataFromJson, ErrorAction));


        if (!webRequest.IsServerProcess)
        {
            
            // リトライボタン押下時の処理を設定
            ErrorPanelButtonDelegateMethod = ExecuteGetRankData;

            // 処理終了
            string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_NETWORK_GET_RANKLIST);
            ErrorAction(ServerConnectErrorID.ERROR_NETWORK_GET_RANKLIST, errorMessage);

            yield break;
        }


    }


    private void LoadRankDataFromJson(byte[] responseText)
    {
        try
        {
            // Json形式のデータをRankDataParamの配列変数に変換して格納
            string res = System.Text.Encoding.GetEncoding("utf-8").GetString(responseText);
            RankData = JsonUtilityHelper.ConvertObjectArrayFromJson<RankDataParam>(res);

            int cnt = 0;
            foreach(var rank in RankData)
            {
                // ランキング表示
                RankTopText[cnt].text = (cnt + 1) + "位"+ " " + rank.user_name　+ " " + rank.score + "点" ;
                cnt++;
            }

        }
        catch (Exception ex)
        {
            ErrorPanelButtonDelegateMethod = ExecuteGetRankData;

            string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_JSON_CONVERT);
            ErrorAction(ServerConnectErrorID.ERROR_JSON_CONVERT, errorMessage + "\r\n" + ex.Message);

            Debug.Log(ex.Message + "\r\n" + ex + "\r\n"+ responseText);
        }
    }


    private void ErrorAction(int errorID, string errorDetail)
    {
        // コールバック
        ErrorActionMethod(errorID, errorDetail);

    }
    #endregion

    #region リトライボタン押下時

    public void OnRetryButton()
    {

        // 設定したリトライ処理を実行
        ErrorPanelButtonDelegateMethod();
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
