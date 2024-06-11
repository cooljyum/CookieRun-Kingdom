using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonManager : MonoBehaviour
{
    public static ClearButtonManager Instance;

    public List<List<Button>> ClearButtons;

    private void Awake()
    {
        Instance = this;

        ClearButtons = new List<List<Button>>();

        for (int i = 0; i < transform.childCount; i++)
        {
            List<Button> row = new List<Button>();
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                Button button = transform.GetChild(i).transform.GetChild(j).GetComponent<Button>();
                row.Add(button);
                button.gameObject.SetActive(false);
            }

            ClearButtons.Add(row);
        }
    }
}
