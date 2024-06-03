using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : CharacterData
{
    [SerializeField] private bool _isBig;
    [SerializeField] private Sprite _enemyPortrait;

    public Sprite EnemyPortrait => _enemyPortrait;
}
