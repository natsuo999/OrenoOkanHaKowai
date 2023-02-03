using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager : Singleton<DialogManager>
{
    protected override void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject DialogTwoButtonCanvasPrefab;
    public GameObject DialogOneButtonCanvasPrefab;
    GameObject InstantiatedDialogObject;


    public GameObject GenerateDialog(string description, string leftButtonName = null, string rightButtonName = null, UnityAction leftEvent = null, UnityAction rightEvent = null)
    {
        InstantiatedDialogObject = Instantiate(DialogTwoButtonCanvasPrefab) as GameObject;
        InstantiatedDialogObject.GetComponent<Canvas>().worldCamera = Camera.main;
        InstantiatedDialogObject.GetComponent<Canvas>().sortingOrder = 99;
        InstantiatedDialogObject.GetComponent<DialogCanvas>().SetContents(description, leftButtonName, rightButtonName, leftEvent , rightEvent);
        return InstantiatedDialogObject;
    }

    public GameObject GenerateDialog(string description, string leftButtonName = null ,UnityAction leftEvent = null)
    {
        InstantiatedDialogObject = Instantiate(DialogOneButtonCanvasPrefab) as GameObject;
        InstantiatedDialogObject.GetComponent<Canvas>().worldCamera = Camera.main;
        InstantiatedDialogObject.GetComponent<Canvas>().sortingOrder = 99;
        InstantiatedDialogObject.GetComponent<DialogCanvas>().SetContents(description, leftButtonName, null, leftEvent, null);
        return InstantiatedDialogObject;
    }

    //Dialogを破壊します
    public void DestroyDialog()
    {
        Destroy(InstantiatedDialogObject);
    }
}
