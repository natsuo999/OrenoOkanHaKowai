using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����N�p�l���̊��N���X
public class PanelRankSceneBase: MonoBehaviour
{
    public Action OnDialogCloseButtonMethod { get; set; }

    public void OnDialogCloseButton()
    {
        OnDialogCloseButtonMethod();
    }

}
