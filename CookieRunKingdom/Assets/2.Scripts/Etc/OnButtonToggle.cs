using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonToggle : MonoBehaviour
{
    public Button ClearButton;
    private CharacterData _characterData;
    public bool isOn = false;

    public void Awake()
    {
        _characterData = base.GetComponent<CharacterData>();
        
    }
    public void OneButtonToggle()
    {
        isOn = !isOn;

        if(!isOn )
        {

        }
    }
}
