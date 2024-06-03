using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _starObjects;
    [SerializeField] private TextMeshProUGUI _stageNameText;

    public void Init(StageData stageData, int starCount = 3)
    {
        _stageNameText.text = stageData.StageName;

        /*foreach (GameObject star in _starObjects)
            star.SetActive(false);

        for (int i = 0; i < starCount; i++)
            _starObjects[i].SetActive(true);*/
    }
}
