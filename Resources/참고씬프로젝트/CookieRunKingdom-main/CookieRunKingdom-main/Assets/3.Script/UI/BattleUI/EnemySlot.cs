using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySlot : MonoBehaviour
{
    [SerializeField] private Image _enemyImage;
    private EnemyData _enemyData;

    public void UpdateUI(EnemyData enemyData)
    {
        _enemyData = enemyData;
        _enemyImage.sprite = _enemyData.EnemyPortrait;
    }
}
