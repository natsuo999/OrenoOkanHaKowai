using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour
{
    public Text MessageText;
    public Button LeftButton;
    public Button RightButton;

    public void SetContents(string _description, string _leftButtonName = null, string _rightButtonName = null, UnityAction leftEvent = null, UnityAction rightEvent = null)
    {
        MessageText.text = _description;


        if (!string.IsNullOrEmpty(_leftButtonName))
        {
            LeftButton.GetComponentInChildren<Text>().text = _leftButtonName;
            LeftButton.gameObject.SetActive(true);
            LeftButton.onClick.RemoveAllListeners();
        }

        if (!string.IsNullOrEmpty(_rightButtonName))
        {
            RightButton.GetComponentInChildren<Text>().text = _rightButtonName;
            RightButton.gameObject.SetActive(true);
            RightButton.onClick.RemoveAllListeners();
        }
       

        if (leftEvent != null)
            LeftButton.onClick.AddListener(leftEvent);

        if (rightEvent != null)
            RightButton.onClick.AddListener(rightEvent);
    }
    
}
