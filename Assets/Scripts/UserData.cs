using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユーザデータを格納するクラス
/// </summary>
[System.Serializable]
public class UserData
{

    public string UserName;
    public int Score;
    public int HighScore;
    public int StageNo;
    public float ScoreGageValue;
    public bool IsMyBsetScore;

    public UserData()
    {
        UserName = "";
        Score = 0;
        HighScore = 0;
        StageNo = 1;
        ScoreGageValue = 0.03f;
        IsMyBsetScore = false;

    }


}
