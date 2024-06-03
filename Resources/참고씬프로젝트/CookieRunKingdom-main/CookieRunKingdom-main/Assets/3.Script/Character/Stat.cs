using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    // 기초 스탯
    private float _baseStat;

    // 깡 능력치 => 레벨이나, 스킬레벨 올릴 때 수치적으로 올라가는 스탯
    private float _kangStats;

    // 퍼센트 능력치 => 랜드마크, 토핑 등으로 올라가는 스탯
    private float _percentStats;

    // 전투중 일시적으로 올라가는 스탯
    private float _tempPercentStats;

    public Stat(float baseStat, float kangStats = 0, float percentStats = 0, float tempPercentStats = 0)
    {
        _baseStat = baseStat;

        _kangStats = kangStats;
        _percentStats = percentStats;
        _tempPercentStats = tempPercentStats;
    }

    public void LevelUpStat()
    {
        _baseStat *= Utils.LevelUpInterest;
    }

    public void EvolutionStat()
    {
        _baseStat *= Utils.EvolutionInterest;
    }

    // 처리 후 스탯
    public int ResultStat => Mathf.RoundToInt((_baseStat + _kangStats) * (1 + ((_percentStats + _tempPercentStats) / 100)));
}
