using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelBase : MonoBehaviour
{

    public void OnTitleButtonAvtion()
    {
        SoundManager.Instance.ChangeBGMVolume(SoundManager.Instance.BGMVolume);
        SoundManager.Instance.PlaySE(SoundManager.SE_Type.Button);
        // パラメータリセット
        UserData data = SaveManager.Instance.LoadSaveData<UserData>();
        data.Score = 0;
        data.StageNo = 1;
        SaveManager.Instance.Save(data);

        //SaveManager.Instance.SetParam(0 /* score */, 1 /* stageNo*/);
        SceneManager.LoadScene(Const.SceneName.TITLE_SCENE);
    }

    public void OnRestartButtonAction()
    {
        SoundManager.Instance.ChangeBGMVolume(SoundManager.Instance.BGMVolume);
        SoundManager.Instance.PlaySE(SoundManager.SE_Type.Button);
        SceneManager.LoadScene(Const.SceneName.TITLE_SCENE);
    }

    public void OnNextButtonAction()
    {
        SoundManager.Instance.ChangeBGMVolume(SoundManager.Instance.BGMVolume);
        SoundManager.Instance.PlaySE(SoundManager.SE_Type.Button);
        SceneManager.LoadScene(Const.SceneName.TITLE_SCENE);
    }

    public void OnRankingButtonAction()
    {
        SoundManager.Instance.ChangeBGMVolume(SoundManager.Instance.BGMVolume);
        SoundManager.Instance.PlaySE(SoundManager.SE_Type.Button);
        SceneManager.LoadScene(Const.SceneName.RANKING_SCENE);
    }
}
