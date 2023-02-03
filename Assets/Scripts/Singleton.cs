using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトン基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    #region Fields変数
    // シングルトンインスタンス
    private static T _instance = null;
    #endregion

    #region Properties
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                // nullの場合全オブジェクトを検索
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogWarning(typeof(T) + "がシーンに存在しません。");
                }
            }
            return _instance;
        }

        private set { _instance = null; }

    }

    // DontDestroyObjectを設定するかどうか
    public bool IsDontDestroyObject()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(Instance);
            return true;
        }
        else if (Instance == this)
        {
            DontDestroyOnLoad(Instance);
            return true;
        }

        return false;
    }
    #endregion

    #region Method
    protected virtual void Awake()
    {
        if (IsDontDestroyObject() == false)
        {
            Destroy(this);
        }
    }

    #endregion

}
