using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnockbackCommand : ICCCommand
{
    private readonly CharacterBattleController _receiver;

    public KnockbackCommand(CharacterBattleController receiver)
    {
        _receiver = receiver;
    }

    public bool IsFinish { get; set; }

    private Sequence _seq;

    public void Execute(Vector3 dir)
    {
        IsFinish = false;

        Vector3 endVector = _receiver.transform.position + dir;
        BattleManager.instance.KnockBackPower = Mathf.RoundToInt(dir.x / Utils.Dir.normalized.x);

        _seq = DOTween.Sequence();
        _seq.Append(_receiver.transform.DOMove(endVector, 0.5f)).SetEase(Ease.OutQuart)
            .OnComplete(Undo);
    }

    public void Undo()
    {
        IsFinish = true;
        _seq.Kill();
    }
}
