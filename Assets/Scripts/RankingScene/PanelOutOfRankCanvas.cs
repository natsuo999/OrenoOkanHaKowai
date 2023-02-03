using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOutOfRankCanvas : PanelRankSceneBase
{
    [SerializeField]
    private Button backButton;

    private void Start()
    {

        backButton.onClick.AddListener(() => base.OnDialogCloseButtonMethod());

    }

}
