using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ランクパネルの基底クラス
public class PanelRankSceneBase: MonoBehaviour
{
    public Action OnDialogCloseButtonMethod { get; set; }

    public void OnDialogCloseButton()
    {
        OnDialogCloseButtonMethod();
    }

}
