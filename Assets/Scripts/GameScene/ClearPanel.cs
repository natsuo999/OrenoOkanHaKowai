using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanel : PanelBase
{
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button nextButton;

    private void Start()
    {
        titleButton.onClick.AddListener(base.OnTitleButtonAvtion);
        nextButton.onClick.AddListener(base.OnNextButtonAction);

    }   

}
