using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRankInCanvas :  PanelRankSceneBase
{
    [SerializeField]
    private Button registButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Text inputFieldText;

    public delegate void CompleteRegistScoreDelegate(string str);
    public CompleteRegistScoreDelegate RegistMethod;

    private void Start()
    {
        registButton.onClick.AddListener(() => RegistMethod(inputFieldText.text.Trim()));
        backButton.onClick.AddListener(() => base.OnDialogCloseButtonMethod());

    }

}
