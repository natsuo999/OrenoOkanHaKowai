using Const;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// UnityWebRequestを使ってサーバ接続するクラス
/// </summary>
public sealed class WebSererRequestClient : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private string ServerAddress = "http://minarai-se.moo.jp/natsuono.com/develop/";
    #endregion

    #region 定数
    /// <summary>サーバ問い合わせ時間</summary>
    private readonly int connectTime = 10;
    private readonly string serverDBConnectPhpFileName = "DBconnect.php";
    #endregion

    #region プロパティ
    // サーバ接続可否
    public bool IsDBServerConnect { get; set; } = false;
    public bool IsServerProcess { get; set; } = false;
    public DataTable ErrorMessageData { get; set; }

    private string ServerAdressDBConnectPhp
    {
        get
        {
            return ServerAddress + serverDBConnectPhpFileName;
        }
    }

    #endregion

    public IEnumerator GetCall(string phpFileNameEx, Action<byte[]> sucessCallBack, Action<int, string> errorCallBack)
    {

        yield return StartCoroutine(WebRequestCallCoroutine("Get", phpFileNameEx, null, sucessCallBack, errorCallBack));

    }

    public IEnumerator PostCall(string phpFileNameEx, Dictionary<string, string> post, Action<byte[]> sucessCallBack, Action<int,string> errorCallBack)
    {
        yield return StartCoroutine(WebRequestCallCoroutine("Post", phpFileNameEx, post, sucessCallBack, errorCallBack));
    }


    private IEnumerator WebRequestCallCoroutine(string method, string phpFileNameEx, Dictionary<string, string> post, Action<byte[]> sucessCallBack, Action<int,string> errorCallBack)
    {
        // サーバ接続処理結果の初期化
        this.IsServerProcess = false;

        UnityWebRequest webRequest;

        if (method == "Get")
        {
            webRequest = UnityWebRequest.Get(ServerAddress + phpFileNameEx);
        }
        else // (method == "POST")
        {
            webRequest = UnityWebRequest.Post(ServerAddress + phpFileNameEx, post);
        }


        webRequest.timeout = connectTime; // タイムアウト時間

        yield return webRequest.SendWebRequest();

        string errorMessage = "";
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:

                // 成功したときの処理
                this.IsServerProcess = true;

                // バイナリで取得
                sucessCallBack(webRequest.downloadHandler.data);

                break;
            case UnityWebRequest.Result.ConnectionError:

                // タイムアウトエラー  
                errorMessage = GetErrorMessage(ServerConnectErrorID.ERROR_RESPONSE_TIME_OVER);
                // 失敗したときの処理
                errorCallBack(ServerConnectErrorID.ERROR_RESPONSE_TIME_OVER, errorMessage + "\r\n" + webRequest.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                // タイムアウトエラー以外
                errorMessage = GetErrorMessage(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
                // 失敗したときの処理
                errorCallBack(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage + "\r\n" + webRequest.error);
                break;
            default:

                // タイムアウトエラー以外
                errorMessage = GetErrorMessage(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
                errorCallBack(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage + "\r\n" + webRequest.error);
                break;
        }



        // メモリ解放
        webRequest.Dispose();



    }

    public string GetErrorMessage(int errorId, string whereColumnName = "ErrorId", string selectColumnName = "ErrorMessage")
    {
        string errorMessage = DataTableManager.SelectStrDataFromDatatable(ErrorMessageData
                                                    , whereColumnName
                                                    , errorId
                                                    , selectColumnName);

        return errorMessage;
    }

    #region サーバ接続処理
    /// <summary>
    /// サーバ接続(GET通信)
    /// </summary>
    /// <param name="url">接続先url</param>
    /// <returns></returns>
    public IEnumerator WebGetRequestCoroutine()
    {


        // DB接続処理結果の初期化
        this.IsDBServerConnect = false;

        // UnityWebRequestを生成
        UnityWebRequest request = UnityWebRequest.Get(ServerAdressDBConnectPhp);
        request.timeout = connectTime; // タイムアウト時間

        // URLに接続して結果が戻ってくるまで待機
        yield return request.SendWebRequest();
        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                this.IsDBServerConnect = true;
                break;
            case UnityWebRequest.Result.ConnectionError:
                break;
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
            default:

                break;
        }

        yield return 0;
    }



    #endregion


}
