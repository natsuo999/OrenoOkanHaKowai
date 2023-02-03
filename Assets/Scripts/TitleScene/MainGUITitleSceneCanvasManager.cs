using Const;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGUITitleSceneCanvasManager : MonoBehaviour
{
    [SerializeField]
    public ButtonManager bttonManagber;
    [SerializeField]
    public GameObject titlePanel;
    [SerializeField]
    public GameObject sinarioPanel;
    [SerializeField]
    public GameObject OkanFukidashiImage;
    [SerializeField]
    public GameObject OreFukidashiImage;
    [SerializeField]
    public GameObject gameRuleImage;
    [SerializeField]
    public GameObject startButton;
    [SerializeField]
    public GameObject levelButtonObjects;
    [SerializeField]
    public GameObject privacyPolicyButton;
    [SerializeField]
    public Text scoreText;


    public void Init(int highScore, int stageNo, DataTable stageData)
    {

        // ボタン設定
        bttonManagber.Initialize(OnGameStartAction, OnSelectStageLevelAction, stageData);
        scoreText.text = "HighScore:" + highScore.ToString();

        // 初回スタート時は処理しない
        // レベル選択ボタンを押下でスタートする
        // ２ステージ目以降はシーン移動と同時に実行
        if (stageNo == 1)
        {
            return;
        }

        privacyPolicyButton.SetActive(false);
        titlePanel.SetActive(false);
        levelButtonObjects.SetActive(false);

        // コルーチンを開始
        StartCoroutine(StartGameCoroutine(stageNo));
    }



    /// <Summary>
    /// コルーチンで処理を待ちます。
    /// </Summary>
    IEnumerator StartGameCoroutine(int stageNo)
    {
        Text okanText = OkanFukidashiImage.transform.GetChild(0).GetComponent<Text>();
        Text orenoText = OreFukidashiImage.transform.GetChild(0).GetComponent<Text>();
        if (stageNo == 2)
        {         
            okanText.text = "ご飯まだ食べてんの！早よ食べて！お皿片付けられへんやろ！";          
            orenoText.text = "はーい…。(え～ゲームいいところなのに…)";
        }
        if (stageNo == 3)
        {
            okanText.text = "宿題ちゃんとやってるん？遊んでばっかりじゃあかんで！";
            orenoText.text = "ちゃんとやってんで～。(宿題はあとでっと…)";
        }

        // 指定した秒数だけ処理を待ちます。(ここでは1.0秒)
        yield return new WaitForSeconds(1.0f);
        sinarioPanel.SetActive(true);
        OkanFukidashiImage.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        OreFukidashiImage.SetActive(true);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // クリック待ち処理

        // 初回時のみゲームルール表示
        if(stageNo == 1)
        {
            OkanFukidashiImage.SetActive(false);
            OreFukidashiImage.SetActive(false);
            gameRuleImage.SetActive(true);
        }


        startButton.SetActive(true);

    }

    #region ゲームスタート押下時
    /// <summary>
    /// ゲームスタート押下時
    /// </summary>
    private void OnGameStartAction()
    {
        // ボタン音
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Button);

        SceneManager.LoadScene(SceneName.GAME_SCENE);
    }
    #endregion

    #region ステージ選択ボタン押下時
    /// <summary>
    /// ステージ選択ボタン押下時
    /// </summary>
    private void OnSelectStageLevelAction()
    {
        // ボタン音
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Button);

        privacyPolicyButton.SetActive(false);
        titlePanel.SetActive(false);
        levelButtonObjects.SetActive(false);

        // コルーチンを開始
        StartCoroutine(StartGameCoroutine(1));

    }
    #endregion

    public void OnPrivacyLinkClick()
    {
        Application.OpenURL("http://natsuono.com/2022/12/30/natsuok%e3%80%80%e3%83%97%e3%83%a9%e3%82%a4%e3%83%90%e3%82%b7%e3%83%bc%e3%83%9d%e3%83%aa%e3%82%b7%e3%83%bc/");
    }
}
