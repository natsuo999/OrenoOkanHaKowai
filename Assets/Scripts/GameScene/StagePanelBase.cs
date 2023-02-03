using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanelBase: MonoBehaviour
{
    public delegate void ClickItemButtoneDelegate(string str);
    public ClickItemButtoneDelegate ItemCliclMethod;

    public void ItemButtonClick(string btnTagName)
    {
        ItemCliclMethod(btnTagName);
    }
}
