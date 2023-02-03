using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Panel : StagePanelBase
{
    [SerializeField]
    private Button[] button;


    private void Start()
    {
        foreach(var btn in button)
        {
            btn.onClick.AddListener(() => { base.ItemButtonClick(btn.tag); });
        }

    }

}
