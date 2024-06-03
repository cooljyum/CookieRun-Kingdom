using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BTServiceBase : BTElementBase
{
    protected System.Action<float> onTickFn;

    public void Init(string _Name, System.Action<float> _OnTickFn)
    {
        Name = _Name;
        onTickFn = _OnTickFn;
    }

    public virtual void OnTick(float deltaTime)
    {
        if (onTickFn != null)
            onTickFn(deltaTime);
    }

    protected override void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0)
    {
        for (int index = 0; index < indentLevel; ++index)
            debugTextBuilder.Append(' ');

        debugTextBuilder.Append($"S: {Name}");
    }
}
