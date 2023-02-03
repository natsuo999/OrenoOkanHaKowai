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

    #region �萔
    private const string serverAddRecordPhpFileName = "AddRecord.php";
    #endregion

    public IRankingScene.ErrorRankingScenenDelegate ErrorActionMethod { get; set; }
    public DataTable ErrorMessageData { get; set; }
    public delegate void CompleteRegistScoreDelegate();
    public CompleteRegistScoreDelegate CompleteMethod;

    #region Fields�ϐ�
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

    #region �����L���O�o�^����
    /// <summary>
    /// �����L���O�o�^�{�^��������
    /// </summary>
    public void RegistScoreRunking(string inputText)
    {

        if (inputText == "")
        {
            inputText = "NoName";
        }

        // input������ێ�
        inputTextStr = inputText;

        // ���͉\�������͍ő�12����(InputField��CharacterLimit�Őݒ�)
        // else if (inputText.Length > 13) { // ���͕s�� }; 

        InstanceStates.rankingScene = this;
        SaveManager.Instance.ProcessID = ServerConnectProcessID.POST_SCORE_DATA;

        // �����L���O�o�^
        StartCoroutine(RegistScoreRunkingCoroutine(inputTextStr));

    }

    private IEnumerator RegistScoreRunkingCoroutine(string inputNameText)
    {

        // �T�[�o�Ƀ|�X�g����f�[�^��ݒ�
        Dictionary<string, string> dic = new Dictionary<string, string>();
        // user_id��Autoincreament�Ŏ����t��
        dic.Add("user_name", inputNameText);
        dic.Add("score", myScore);

        // �X�R�A+���O����Z�o�����n�b�V���l���T�[�o�ɑ��M����
        dic.Add("hashValue", GetHash(inputNameText + myScore + "club_y"));

        // �T�[�o�ʐM����������܂őҋ@
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

    // �G���[���̃A�N�V�������`
    private void ErrorAction(int errorID, string errorDetail)
    {

        ErrorActionMethod(errorID, errorDetail);

    }



    // �����L���O�o�^���̃A�N�V�������`
    private void RegistRunkAction(byte[] results)
    {
        if (results != null && results.Length > 0)
        {
            string res = System.Text.Encoding.GetEncoding("utf-8").GetString(results);

            // �o�^���s�����ꍇ
            // Panel������
            ResetPanel();

            // ���g���C�{�^���������̏�����ݒ�
            ErrorPanelButtonDelegateMethod = RegistScoreRunking;

            //string errorMessage =  "AddRecordPhpErr:" + res;
            string errorMessage = GetErrorMessage(ErrorMessageData, ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER);
            ErrorActionMethod(ServerConnectErrorID.ERROR_EXCEPT_RESPONSE_TIME_OVER, errorMessage + "\r\n" +res);

        }
        else
        {
            // �o�^���������ꍇ
            // Panel������
            ResetPanel();

            // �o�^���܂����_�C�A���O�\��
            CompleteMethod();

        }
    }
    #endregion


    public void ResetPanel()
    {
        rankInPanel.SetActive(false);
        rankOutPanel.SetActive(false);

    }


    #region ���g���C�{�^��������
    public void OnRetryButton()
    {
        Debug.Log("RetryButton");

        // ���g���C�{�^���������̏��������s
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
