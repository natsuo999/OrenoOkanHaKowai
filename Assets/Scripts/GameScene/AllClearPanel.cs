using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllClearPanel : PanelBase
{
    [SerializeField]
    private Button rankingButton;
    [SerializeField]
    private Button titleButton;

    private void Start()
    {
        rankingButton.onClick.AddListener(base.OnRankingButtonAction);
        titleButton.onClick.AddListener(base.OnTitleButtonAvtion);


    }
}
