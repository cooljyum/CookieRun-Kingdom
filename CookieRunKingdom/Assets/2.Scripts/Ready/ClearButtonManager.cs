using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonManager : MonoBehaviour
{
    public ClearButtonManager Instance;

    public List<Button> ClearButtons;

    private void Awake()
    {
        Instance = this;

        ClearButtons = new List<Button>();

        for (int i = 0; i < transform.childCount; i++) 
        {
            ClearButtons.Add(transform.GetChild(i).GetComponent<Button>());            
        }

        foreach(Button button in ClearButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
