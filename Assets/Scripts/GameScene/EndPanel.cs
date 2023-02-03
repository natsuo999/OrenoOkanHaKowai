using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : PanelBase
{
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button restartButton;

    private void Start()
    {
        titleButton.onClick.AddListener(base.OnTitleButtonAvtion);
        restartButton.onClick.AddListener(base.OnRestartButtonAction);

    }

}
