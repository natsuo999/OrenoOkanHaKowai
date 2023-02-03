using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    public float Time { get; private set; }
    public float BGMVolume { get; } = 0.6f;
    public AudioSource audioBGMScource;
    public AudioSource audioSESource;
    public AudioClip[] audioBGMClips;
    public AudioClip[] audioSEClips;

    public enum BGM_Type
    {
        Title,
        Game,
    }

    public enum SE_Type
    {
        Ashioto,
        AllClear,
        Success,
        GameOver,
        DoorOpen,
        Fail1,
        Fail2,
        Fire,
        Bumb,
        DoorClose,
        Button

    }

    protected override void Awake()
    {
        // 既に同一のインスタンスが存在していた場合
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }



    //音楽を鳴らしたいときにSoundManager.Instance.PlayBGM();を実行する
    public void PlayBGM(BGM_Type buttonType)
    {
        switch(buttonType)
        {
            case BGM_Type.Title:
                audioBGMScource.PlayOneShot(audioBGMClips[0]);
                break;
            case BGM_Type.Game:
                audioBGMScource.PlayOneShot(audioBGMClips[1]);
                break;


        }
        
    }

    public void ChangeBGMVolume(float volume)
    {
        audioBGMScource.volume = volume;

    }

    public void PlaySE(SE_Type buttonType)
    {
        switch (buttonType)
        {
            case SE_Type.Ashioto:
                audioSESource.PlayOneShot(audioSEClips[0]);
                break;
            case SE_Type.AllClear:
                audioSESource.PlayOneShot(audioSEClips[1]);
                break;
            case SE_Type.Success:
                audioSESource.PlayOneShot(audioSEClips[2]);
                break;
            case SE_Type.GameOver:
                audioSESource.PlayOneShot(audioSEClips[3]);
                break;
            case SE_Type.DoorOpen:
                audioSESource.PlayOneShot(audioSEClips[4]);
                break;
            case SE_Type.Fail1:
                audioSESource.PlayOneShot(audioSEClips[5]);
                break;
            case SE_Type.Fail2:
                audioSESource.PlayOneShot(audioSEClips[6]);
                break;
            case SE_Type.Fire:
                audioSESource.PlayOneShot(audioSEClips[7]);
                break;
            case SE_Type.Bumb:
                audioSESource.PlayOneShot(audioSEClips[8]);
                break;
            case SE_Type.DoorClose:
                audioSESource.PlayOneShot(audioSEClips[9]);             
                break;
            case SE_Type.Button:
                audioSESource.PlayOneShot(audioSEClips[10]);
                break;

        }



    }

    public void StopBGM()
    {
        
        audioBGMScource.Stop();
        Time = audioBGMScource.time;

        audioSESource.Stop();


    }



}

