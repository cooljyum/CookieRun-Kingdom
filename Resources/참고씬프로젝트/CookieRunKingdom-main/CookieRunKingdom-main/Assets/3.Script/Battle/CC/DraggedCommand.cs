using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DraggedCommand : ICCCommand
{
    private readonly CharacterBattleController _receiver;

    public DraggedCommand(CharacterBattleController receiver)
    {
        _receiver = receiver;
    }

    public bool IsFinish { get; set; }

    private Sequence _seq;

    public void Execute(Vector3 dir)
    {
        IsFinish = false;

        Vector3 endVector = _receiver.transform.position + dir;

        _seq = DOTween.Sequence();
        _seq.Append(_receiver.transform.DOMove(endVector, 0.1f)).SetEase(Ease.Linear)
            .OnComplete(Undo);
    }

    public void Undo()
    {
        IsFinish = true;
        _seq.Kill();
    }
}
