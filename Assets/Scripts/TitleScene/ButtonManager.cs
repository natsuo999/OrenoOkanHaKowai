using Const;
using System;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public enum StageType
    {
        bigginer = 1,
        midium,
        high

    }

    [SerializeField]
    private Button gameStartButtons;
    [SerializeField]
    private Button[] levelButtons;


    private Text buttonText;
    private UnityAction OnLevelButtonMethod;

    public void Initialize(UnityAction OnGameStartAction, UnityAction OnSelectLevelAction, DataTable stageData)
    {
        
        // ゲームスタートボタンにボタン押下時の処理を設定
        gameStartButtons.onClick.AddListener(OnGameStartAction);

        this.OnLevelButtonMethod = OnSelectLevelAction;

        int cnt = 0;
        foreach (Button levelBtn in levelButtons)
        {
            Button btn = levelBtn.GetComponent<Button>();
            buttonText = btn.transform.GetChild(0).GetComponent<Text>();
            if (cnt == 0)
            {
                buttonText.text = TitleSceneConsts.BUTTON_1;
            }
            else if (cnt == 1)
            {
                buttonText.text = TitleSceneConsts.BUTTON_2;
            }
            else
            {
                buttonText.text = TitleSceneConsts.BUTTON_3;
            }

            // レベル選択ボタンにボタン押下時の処理を設定
            btn.onClick.AddListener(() => this.OnSelectStageLevel(btn, stageData));

            cnt++;
        }

    }

    public void OnSelectStageLevel(Button button, DataTable stageData)
    {

        int stageLevel = 0;

        //　ボタンの文字列に応じて書き換える
        string str = button.transform.GetChild(0).GetComponent<Text>().text;
        if (str == TitleSceneConsts.BUTTON_1)
        {
            stageLevel = (int)StageType.bigginer;
        }
        if (str == TitleSceneConsts.BUTTON_2)
        {
            stageLevel = (int)StageType.midium;

        }
        if (str == TitleSceneConsts.BUTTON_3)
        {
            stageLevel = (int)StageType.high;

        }

        string data = DataTableManager.SelectStrDataFromDatatable(stageData, "StageLevel", stageLevel, "ScoreGageValue");
      
        // 選択したレベルのスコアゲージ値を保持
        UserData userData = SaveManager.Instance.LoadSaveData<UserData>();
        userData.ScoreGageValue = float.Parse(data);
        SaveManager.Instance.Save(userData);
   

        OnLevelButtonMethod();
    }
}
