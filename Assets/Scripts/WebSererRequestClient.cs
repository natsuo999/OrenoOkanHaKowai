using Const;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// UnityWebRequest���g���ăT�[�o�ڑ�����N���X
/// </summary>
public sealed class WebSererRequestClient : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private string ServerAddress = "http://minarai-se.moo.jp/natsuono.com/develop/";
    #endregion

    #region �萔
    /// <summary>�T�[�o�₢���킹����</summary>
    private readonly int connectTime = 10;
    private readonly string serverDBConnectPhpFileName = "DBconnect.php";
    #endregion

    #region �v���p�e�B
    // �T�[�o�ڑ���
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
        // �T�[�o�ڑ��������ʂ̏�����
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


        webRequest.timeout = connectTime; // �^�C���A�E�g����

        yield return webRequest.SendWebRequest();

        string errorMessage = "";
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:

                // ���������Ƃ��̏���
                this.IsServerProcess = true;

                // �o�C�i���Ŏ擾
                sucessCallBack(webRequest.downloadHandler.data);

                break;
            case UnityWebRequest.Result.ConnectionError:

                // �^�C���A�E�g�G���[  
                errorMessage = GetErrorMessage(ServerConnectErrorID.ERROR_RESPONSE_TIME_OVER);
                // ���s�����Ƃ��̏���
                errorCallBack(ServerConnectErrorID.ERROR_RESPONSE_TIME_OVER, errorMessage + "\r\n" + webRequest.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                // �^�C���A�E�g�G���[�ȊO
                errorMessage = GetErrorMessage(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
                // ���s�����Ƃ��̏���
                errorCallBack(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage + "\r\n" + webRequest.error);
                break;
            default:

                // �^�C���A�E�g�G���[�ȊO
                errorMessage = GetErrorMessage(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
                errorCallBack(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage + "\r\n" + webRequest.error);
                break;
        }



        // ���������
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

    #region �T�[�o�ڑ�����
    /// <summary>
    /// �T�[�o�ڑ�(GET�ʐM)
    /// </summary>
    /// <param name="url">�ڑ���url</param>
    /// <returns></returns>
    public IEnumerator WebGetRequestCoroutine()
    {


        // DB�ڑ��������ʂ̏�����
        this.IsDBServerConnect = false;

        // UnityWebRequest�𐶐�
        UnityWebRequest request = UnityWebRequest.Get(ServerAdressDBConnectPhp);
        request.timeout = connectTime; // �^�C���A�E�g����

        // URL�ɐڑ����Č��ʂ��߂��Ă���܂őҋ@
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
