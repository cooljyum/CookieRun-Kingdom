using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirborneCommand : ICCCommand
{
    private readonly CharacterBattleController _receiver;

    public AirborneCommand(CharacterBattleController receiver)
    {
        _receiver = receiver;
    }

    public bool IsFinish { get; set; }

    private Sequence _seq;

    public void Execute(Vector3 dir)
    {
        IsFinish = false;

        _seq = DOTween.Sequence();
        _seq.Append(_receiver.transform.GetChild(0).DOLocalMoveY(dir.y, 0.5f)).SetEase(Ease.OutCubic)
            .Append(_receiver.transform.GetChild(0).DOLocalMoveY(0, 0.5f)).SetEase(Ease.InCubic)
            .OnComplete(Undo);
    }

    public void Undo()
    {
        _receiver.transform.GetChild(0).localPosition = Vector3.zero;
        IsFinish = true;
        _seq.Kill();
    }
}
