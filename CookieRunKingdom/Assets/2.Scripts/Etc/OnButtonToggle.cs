using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonToggle : MonoBehaviour
{
    public Button ClearButton;
    private StandingCharacter _character;
    public bool isOn = false;

    public void Awake()
    {
        _character = GetComponent<StandingCharacter>();        
    }
    public void OneButtonToggle()
    {
        isOn = !isOn;

        if(!isOn )
        {

        }
    }
}
