using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStat
{
    hp, attack, defense, criticlal, size
}

public class CharacterStat : MonoBehaviour
{
    // 구현할건 이거
    // 전투력, 체력, 공격력, 방어력, 치명타 확률


    // 원래는 이거
    // 공격력, 방어력, 체력, 공격속도, 치명타 확률
    // 쿨타임, 피해감소, 치명타 피해감소,
    // 이로운 효과 증가, 해로운 효과 감소

    // 스킬레벨업하면 약 4% 복리 증가
    // 레벨업하면 약 4.5% 복리 증가
    // 승급하면 약 5% 복리 증가

    public int powerStat => attackStat.ResultStat + defenseStat.ResultStat;
    public Stat hpStat;
    public Stat attackStat;
    public Stat defenseStat;
    public Stat criticalStat;

    private Stat[] _stats;
    private BaseController _controller;

    public void Init(BaseController controller)
    {
        _controller = controller;

        hpStat = new Stat(_controller.Data.HpStat);
        attackStat = new Stat(_controller.Data.AttackStat);
        defenseStat = new Stat(_controller.Data.DefenseStat);
        criticalStat = new Stat(_controller.Data.CriticalStat);

        _stats = new Stat[(int)EStat.size];
        _stats[0] = hpStat;
        _stats[1] = attackStat;
        _stats[2] = defenseStat;
        _stats[3] = criticalStat;
    }

    public void LevelUP()
    {
        for(int i = 0; i < _stats.Length; i++)
        {
            _stats[i].LevelUpStat();
        }
    }

    public void Evolution()
    {
        for(int i = 0; i < _stats.Length; i++)
        {
            _stats[i].EvolutionStat();
        }
    }
}
