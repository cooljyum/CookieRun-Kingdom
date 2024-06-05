using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyManager : MonoBehaviour
{
    static public ReadyManager Instance;

    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject deckPanel;

    private void Awake()
    {
        Instance = this;

        //Transform[] children = GetComponentsInChildren<Transform>();
        //
        //foreach (Transform child in children)
        //{
        //    if(child.name == "BattleReadyPanel")
        //    {
        //        readyPanel = child.gameObject;
        //    }
        //    else if(child.name == "DeckSettingPanel")
        //    {
        //        deckPanel = child.gameObject;
        //    }
        //}
    }

    public void OnClickDeckSetting()
    {
        readyPanel.SetActive(false);
        deckPanel.SetActive(true);
    }    
}
