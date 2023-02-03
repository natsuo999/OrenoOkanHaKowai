using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Data;

public sealed class MainGUIGameSceneCanvasManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private Stage1Panel stagePanel1;
    [SerializeField]
    private Stage2Panel stagePanel2;
    [SerializeField]
    private Stage3Panel stagePanel3;
    [SerializeField]
    private GameObject endPanelObj;
    [SerializeField]
    private GameObject clearPanelObj;
    [SerializeField]
    private GameObject allClearPanelObj;
    [SerializeField]
    private float rimitTime = 10.0f;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text stageText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Image clockImage;
    [SerializeField]
    private Sprite playerBedImage;
    [SerializeField]
    private Sprite playerEatImage;
    [SerializeField]
    private Sprite playerStudyImage;
    [SerializeField]
    private Sprite playerDamegeImage;
    [SerializeField]
    private Sprite playerImageGame;
    [SerializeField]
    private Sprite playerImageSumaho;
    [SerializeField]
    private Sprite playerImageVRGame;
    [SerializeField]
    private Sprite playerImageBook;
    [SerializeField]
    private Sprite playerImageGameCenter;
    [SerializeField]
    private Sprite playerImageJikkyo;
    [SerializeField]
    private Sprite clockImage_0930;
    [SerializeField]
    private Sprite clockImage_1000;
    [SerializeField]
    private Sprite clockImage_1030;
    [SerializeField]
    private Sprite clockImage_1100;
    [SerializeField]
    private Sprite clockImage_1130;
    [SerializeField]
    private Sprite clockImage_1200;
    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private GameObject playerLifeImage1;
    [SerializeField]
    private GameObject playerLifeImage2;
    [SerializeField]
    private GameObject playerLifeImage3;
    [SerializeField]
    private GameObject enemyObj;
    [SerializeField]
    private GameObject enemyPunchObj;
    [SerializeField]
    private Sprite enemyAngryImage1;
    [SerializeField]
    private Sprite enemyAngryImage2;
    [SerializeField]
    private Sprite enemyAngryImage3;
    [SerializeField]
    private GameObject doorCloseImage;
    [SerializeField]
    private GameObject doorOpenImage;
    [SerializeField]
    private Slider scoreGageSlider;
    [SerializeField]
    private GameObject bounsImage;
    [SerializeField]
    private GameObject bounsButton;
    #endregion

    #region Field変数
    private int stageNo = 1;
    private float scoreGageValue = 0.01f;
    public float bounsTime = 3f;
    private int score = 0; // カウント用

    private Sprite playerImage_;
    private int stageNoMax = 3;
    private float time = 0f;
    private float stopBgmTime = 0f; // bgmの経過時間

    private float randomTime;
    private bool isPlayEnemyAppearWarningSound = false;
    private bool isEnemyAppear = false;
    private bool isTimeCount = true;
    private int playerLifePt = 3;

    private bool isGameEnd = false;
    private bool isPlayerAwakeState = false;
    private bool isBounsTime = false;
    private float scoreGageValue_ = 0.01f;
    private float randomTimeByStageLevel = 0.01f;


    private Vector3[] buttonVecArray;
    private Transform[] buttonObjArray;
    private int score_;
    private int myBestScore;
    private UserData _userData;
    private float bgmVolume;
    #endregion

    #region 初期設定
    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="userData">ユーザデータ</param>
    public void Initialize(UserData userData, DataTable stageParamData)
    {

    
        // 値をField変数に保持
        SetFieldValue(userData, userData.HighScore, userData.Score, userData.ScoreGageValue, userData.StageNo, stageParamData);

        // 対象ステージ以外は非表示
        StagePanelBase[] stageArray = new StagePanelBase[] { stagePanel1, stagePanel2, stagePanel3 };
        for (int i = 1; i < userData.StageNo; i++)
        {
            stageArray[i - 1].gameObject.SetActive(false);
        }

        // 対象のステージパネルを設定
        SetUpProcessGamePanel(userData.StageNo, stageArray[userData.StageNo - 1]);

        // (ステージ共通)ボーナスボタンの処理を設定
        Button btn = bounsButton.GetComponent<Button>();
        btn.onClick.AddListener(() => this.OnItemButtonAction(btn.tag));

        // (ステージ共通)テキスト設定
        SetUpStageTextValue(userData.Score, userData.StageNo);

    }
    #endregion

    #region Update関数
    void Update()
    {
        
        // 経過時間によって時計の画像を変更
        ChangeClockImage(time);

        if (!isGameEnd)
        {
            if (time >= rimitTime)
            {
                isGameEnd = true;
                ExecuteGameEndAction();
            }
            else
            {
                
                if(isTimeCount)
                {
                    // カウントUp
                    time += Time.deltaTime;

                    // 残り時間
                    float leftTime = rimitTime - time;
                    if(leftTime > 0)
                    {
                        SetTimeText(leftTime);

                        if (!isBounsTime)
                        {
                            // ランダムでボーナスボタン表示
                            if (UnityEngine.Random.Range(0, 300) == 0)
                            {
                                bounsButton.SetActive(true);
                            }
                        }
                    }
              
                }

                if(isBounsTime)
                {
                    // カウントダウン
                    bounsTime -= Time.deltaTime;
                    if (bounsTime <= 0)
                    {
                        isBounsTime = false;
                        ExcuteBounsTimeEndAction();
                    }
                   
                }

                if (isPlayerAwakeState && !isEnemyAppear)
                {                 
                    // スコアゲージ増加
                    scoreGageSlider.value += scoreGageValue;

                    // ステージクリアした場合
                    if (scoreGageSlider.value >= scoreGageSlider.maxValue)
                    {
                        isGameEnd = true;
                        ExcuteGameClearAction();
                    }
                }
            }
        
        }

        // 経過時間 >= (ランダム時間 - *秒)
        // 敵が出現する***秒前(ランダム)から足音を鳴らす
        if ( time >= (randomTime - GetRandomTime(0.1f, 2f)) && !isPlayEnemyAppearWarningSound)
        {
            isPlayEnemyAppearWarningSound = true;

            // 敵出現の足音を鳴らす
            // と同時にBGMの音量を下げる
            if(SoundManager.Instance != null)
            {
                bgmVolume = SoundManager.Instance.audioBGMScource.volume;
                SoundManager.Instance.ChangeBGMVolume(0);
            }        
            SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Ashioto);

        }

　　　　// 経過時間 >= ランダム時間
        if ( time >= randomTime && !isEnemyAppear)
        {
            // タイムカウントStop
            isTimeCount = false;

            // 敵出現演出
            isEnemyAppear = true;
            ExcuteEnemyAppearAction(playerLifePt);

            if (isPlayerAwakeState)
            {
                // ライフ画像表示
                playerLifePt -= 1;
                ExcutePlayerDamageAction(playerLifePt);

                if(playerLifePt <= 0)
                {
                    isGameEnd = true;

                    // スコア保持
                    score = score_; // ゲームスタート前のスコアを再設定
                    SaveData(_userData, score, stageNo);

                    // 時間を少しおいてからEndingを表示
                    // コルーチンを開始
                    StartCoroutine(GameOverPerfomanceCoroutine());
                }

            }
        }

    }

    private float GetRandomTime(float minTime , float maxTime)
    {

        return UnityEngine.Random.Range(minTime, maxTime);

    }

    private void ExcuteGameClearAction()
    {      
        // 足音ストップ
        SoundManager.Instance?.StopBGM();
        //SoundManager.Instance.ChangeBGMVolume(bgmVolume);

        // ステージカウントup
        stageNo += 1;
        if (stageNo > stageNoMax)
        {

            // クリア音も一緒に鳴らす
            SoundManager.Instance?.PlaySE(SoundManager.SE_Type.AllClear);
            allClearPanelObj.SetActive(true);

            // スコア保存
            GenerateSaveData(_userData, myBestScore, score);

        }
        else
        {
            // クリア音も一緒に鳴らす
            SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Success);
            clearPanelObj.SetActive(true);

            // 次のステージへ進むため、スコアとステージNoを保持
            SaveData(_userData, score, stageNo);


        }
    }

    public void SaveData(UserData data, int score, int stageNo)
    {
        // 次のステージへ進むため、スコアとステージNoを保持
        data.Score = score;
        data.StageNo = stageNo;
        SaveManager.Instance.Save(data);

     
    }

    // セーブ
    public void GenerateSaveData(UserData saveData, int myBestScore, int score)
    {
 
        saveData.Score = score;
        if (myBestScore < score)
        {
            saveData.IsMyBsetScore = true;
            saveData.HighScore = score;
        }

        SaveManager.Instance.Save(saveData);
    }

    private void ExcuteBounsTimeEndAction()
    {
        
        bounsTime = 3f; // カウントリセット
        scoreGageValue = scoreGageValue_; // ボーナスタイム終了したら元に戻す
        bounsImage.SetActive(false);

    }

    private void ExecuteGameEndAction()
    {
        SetTimeText(0.00f);

        // 時間切れでゲームオーバー
        endPanelObj.SetActive(true);

        // 足音ストップ
        SoundManager.Instance?.StopBGM();

        // クリア音も一緒に鳴らす
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.GameOver);
        //SoundManager.Instance.ChangeBGMVolume(bgmVolume);

        score = score_; // ゲームスタート前のスコアを再設定

        // スコア保持
        SaveData(_userData, score, stageNo);

    }

    /// <summary>
    /// 残り時間のテキストを設定
    /// </summary>
    /// <param name="time">表示時間</param>
    private void SetTimeText(float time)
    {
        timeText.text = "Time:" + time.ToString("f2"); // 桁数調整
    }

    private void ExcuteEnemyAppearAction(int plyaerlife)
    {

        // BGMストップ
        SoundManager.Instance?.StopBGM();
        // BGMストップした時間を保持
        if(SoundManager.Instance != null)
        {
            stopBgmTime = SoundManager.Instance.Time;

        }      

        // ドア音も一緒に鳴らす
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.DoorOpen);

        doorCloseImage.SetActive(false);
        doorOpenImage.SetActive(true);

        // playerライフによって敵画像を変更
        if (plyaerlife == 2)
        {
            enemyObj.GetComponent<Image>().sprite = enemyAngryImage1;
        }
        if (plyaerlife == 1)
        {
            enemyObj.GetComponent<Image>().sprite = enemyAngryImage2;
        }

        enemyObj.SetActive(true);
    }

    /// <summary>
    /// プレイヤーがダメージ受けたときのアクション
    /// </summary>
    /// <param name="playerlife"></param>
    private void ExcutePlayerDamageAction(int playerlife)
    {

        if (playerlife == 2)
        {
            // ガーン音も一緒に鳴らす
            SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Fail1);
            playerLifeImage3.SetActive(false);

        }
        if (playerlife == 1)
        {
            // ガーン音も一緒に鳴らす
            SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Fail1);
            playerLifeImage2.SetActive(false);
        }
        if (playerlife <= 0)
        {
            playerLifeImage1.SetActive(false);

        }
    }


    /// <Summary>
    /// ゲームオーバー演出のコルーチン
    /// </Summary>
    IEnumerator GameOverPerfomanceCoroutine()
    {

        // 指定した秒数だけ処理を待つ
        yield return new WaitForSeconds(1.0f);

        // ガーン音も一緒に鳴らす
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Fail2);

        // ボーという音も一緒に鳴らす
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Fire);

        // 敵画像を変更
        enemyObj.GetComponent<Image>().sprite = enemyAngryImage3;
        Vector3 vec = new Vector3(1.2f, 1.2f, 1.2f);
        enemyObj.transform.localScale = vec;

        // 徐々にEnmey画像を大きく
        for (float i = vec.x; i < 2.2; i += 0.1f)
        {
            enemyObj.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.1f);

        }

        // ボンっという音も一緒に鳴らす
        SoundManager.Instance?.PlaySE(SoundManager.SE_Type.Bumb);

        // ビヨヨーンみたいな感じで、指定した値と現在の値を行き来
        enemyPunchObj.SetActive(true);     
        enemyPunchObj.transform.DOPunchScale(new Vector3(1.8f, 1.8f), 1f);

        // プレイヤーがダメージを受けた画像に変更し、吹っ飛ばされる
        playerImage.sprite = playerDamegeImage;
        playerObj.transform.Rotate(new Vector3(0, 180));
        playerObj.transform.DOMove(new Vector3(playerObj.transform.position.x -100, playerObj.transform.position.y), 1.0f);

        // クリック待ち処理
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); 

        // Endingパネル表示
        endPanelObj.SetActive(true);


    }
    #endregion

    #region Initに関する処理
    private void SetFieldValue(UserData userData, int highScore, int userScore, float userScoreGageValue, int userStageNo, DataTable stageParamData)
    {
        _userData = userData;
        myBestScore = highScore;
        score = userScore; // 再挑戦した人用にゲームスタート前のスコアを保持しておく
        scoreGageValue = userScoreGageValue;
        stageNo = userStageNo;

        // 難易度として敵の出現ランダム時間の上限も変更
        // (レベルが上がるに連れて上限が小さくなり、出現頻度が高くなる)
        string difficultStr = DataTableManager.SelectStrDataFromDatatable(stageParamData, "StageNo", userStageNo, "Difficult");
        randomTimeByStageLevel = float.Parse(difficultStr);
        
        // ランダム時間の初期値
        randomTime = GetRandomTime(1f, randomTimeByStageLevel);

        // 元の値を保持しておく
        score_ = score;
        scoreGageValue_ = scoreGageValue;
        
    }

    private void SetUpProcessGamePanel(int userStageNo, StagePanelBase stagePanel)
    {

        // 対象ステージのアイテムクリック処理を設定
        stagePanel.ItemCliclMethod = this.OnItemButtonAction;

        GameObject targetStagePanelObj = stagePanel.gameObject;
        targetStagePanelObj.SetActive(true);

        SetEventTrigger(targetStagePanelObj, this.PanelTapAction);

        SetButtonObjVec(targetStagePanelObj);

        playerImage.sprite = GetPlayerImageByStageLevel(userStageNo);
        playerImage_ = playerImage.sprite;

    }


    private void SetEventTrigger(GameObject targetObj, Action targetAction)
    {
        /* memo
          画面タッチ は Input.GetMouseButtonDown(0)で実装した場合、
          ボタン押下イベントと識別できないため、
          画面サイズのImageにEventTriggerを追加して識別する */

        // EventTriggerコンポーネントを取得
        EventTrigger eventTrigger = targetObj.gameObject.GetComponent<EventTrigger>();

        // PointerDown(押した瞬間に実行する)イベントタイプを設定
        EventTrigger.Entry entry = new EventTrigger.Entry();    
        entry.eventID = EventTriggerType.PointerDown; 

        // 関数を設定
        entry.callback.AddListener((x) =>
        {
            targetAction();
        });

        //イベントの設定をEventTriggerに反映
        eventTrigger.triggers.Add(entry);
    }

    private Sprite GetPlayerImageByStageLevel(int userStageNo)
    {
        // ステージによって主人公の画像を変更
        if (userStageNo == 1)
        {
            return playerBedImage;
        }
        if (userStageNo == 2)
        {
            return playerEatImage;
        }
        if (userStageNo == 3)
        {
            return playerStudyImage;
        }

        return playerBedImage;
    }

    private void SetButtonObjVec(GameObject targetObj)
    {
        // ステージのオブジェクトを全て取得し、対象のボタンの配置のみ取得する
        Transform[] children = targetObj.GetComponentsInChildren<Transform>();

        // ボタンを持つオブジェクトのみ取得
        var resObjIer = children.Where(x => x.gameObject.GetComponent<Button>() != null);
        int cnt = resObjIer.Count();
        buttonVecArray = new Vector3[cnt];
        buttonObjArray = new Transform[cnt];

        int buttonCnt = 0;
        foreach (Transform ob in resObjIer)
        {
            if (ob.tag != "Untagged")
            {
                buttonVecArray[buttonCnt] = ob.transform.position;
                buttonObjArray[buttonCnt] = ob;
                buttonCnt++;
            }
        }
    }

    private void SetUpStageTextValue(int score, int stageNo)
    {
        scoreText.text = "SCORE:" + score;
        stageText.text = "STAGE:" + stageNo;
    }


    /// <summary>
    /// 時計画像の変更
    /// </summary>
    /// <param name="progressTime">経過時間</param>
    private void ChangeClockImage(float progressTime)
    {
        // 経過時間によって時計の画像を変更
        string spName = clockImage.sprite.name;

        if (progressTime >= 5f && spName == "clock_0900")
        {
            clockImage.sprite = clockImage_0930;
        }
        if (progressTime >= 10f && spName == "clock_0930")
        {
            clockImage.sprite = clockImage_1000;
        }
        if (progressTime >= 15f && spName == "clock_1000")
        {
            clockImage.sprite = clockImage_1030;
        }
        if (progressTime >= 20f && spName == "clock_1030")
        {
            clockImage.sprite = clockImage_1100;
        }
        if (progressTime >= 25f && spName == "clock_1100")
        {
            clockImage.sprite = clockImage_1130;
        }
        if (progressTime >= 30f && spName == "clock_1130")
        {
            clockImage.sprite = clockImage_1200;
        }
    }
    #endregion

    #region 画面タップ時の関数
    /// <summary>
    /// 画面タップ時(ボタン以外)に呼び出される関数
    /// </summary>
    private void PanelTapAction()
    {
        Debug.Log("画面タップ");

        // ゲームオーバーのときのみ以降のタップ処理は行わない
        if(isGameEnd)
        {
            return;
        }

        // 元の画像に戻す
        playerImage.sprite = playerImage_;
        isPlayerAwakeState = false;

        // ボタンを並び変えて再配置
        RelocateItemButton();

        // 敵出現時かつゲームオーバーでないときのみ
        if (isEnemyAppear)
        {
            // BGMストップした時間から再祖開始
            if(SoundManager.Instance != null)
            {
                SoundManager.Instance.audioBGMScource.time = stopBgmTime;
            }       
            SoundManager.Instance?.PlayBGM(SoundManager.BGM_Type.Game);
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.ChangeBGMVolume(bgmVolume);
            }

            doorCloseImage.SetActive(true);
            doorOpenImage.SetActive(false);
            enemyObj.SetActive(false);

            // ドア音も一緒に鳴らす
            SoundManager.Instance?.PlaySE(SoundManager.SE_Type.DoorClose);

            isPlayEnemyAppearWarningSound = false;

            isEnemyAppear = false;

            // タイムカウント開始
            isTimeCount = true;

            // 経過時間 + ランダム時間
            randomTime = time + GetRandomTime(2f, randomTimeByStageLevel);
            
        }


    }
    #endregion

    #region アイテムボタン押下時に呼び出される関数
    /// <summary>
    /// アイテムボタン押下時に呼び出される関数
    /// </summary>
    /// <param name="pushButtonTagName">押下されたボタンのタグ名</param>
    private void OnItemButtonAction(string pushButtonTagName)
    {
        Debug.Log("ボタンタップ");

        // 敵出現時は以降のタップ処理は行わない
        if (isEnemyAppear)
        {
            return;
        }

        if (pushButtonTagName == "Bouns")
        {
            // ボーナスボタンかつplayerがゲージをためている最中のみ２倍
            if(isPlayerAwakeState)
            {
                scoreGageValue = scoreGageValue * 2;
                bounsButton.SetActive(false); // 一度押すと消える
                bounsImage.SetActive(true);
                isBounsTime = true;
            }

            // ボーナスボタン押下時はゲージはカウントしない
            return;
        }

        // 元のイメージを保持
        //playerImage_ = playerImage.sprite;

        bool isValidItem = true;
        switch (pushButtonTagName)
        {
            case "Game":
                playerImage.sprite = playerImageGame;
                break;
            case "Sumaho":
                playerImage.sprite = playerImageSumaho;
                break;
            case "VRGame":
                playerImage.sprite = playerImageVRGame;
                break;
            case "Book":
                playerImage.sprite = playerImageBook;
                break;
            case "GameCenter":
                playerImage.sprite = playerImageGameCenter;
                break;
            case "HeadSet":
                playerImage.sprite = playerImageJikkyo;
                break;
            default: // 上記以外は無効なアイテム
                isValidItem = false;
                break;

        }

        // ボタンを並び変えて再配置
        RelocateItemButton();

        if (!isValidItem) return;

        score +=100;
        scoreText.text = "Score:" + score;
        isPlayerAwakeState = true;

      
    }
    #endregion

    /// <summary>
    /// ボタンを並び変えて再配置
    /// </summary>
    private void RelocateItemButton()
    {
        
        int cnt = 0;
        var buttonVecArray_ = ManipulateArrayParts.ShuffleArrayContents(buttonVecArray); // todo 例外処理　cathする
        foreach (var btn in buttonObjArray)
        {
            btn.transform.position = buttonVecArray_[cnt];
            cnt++;
        }
    }
    


}
