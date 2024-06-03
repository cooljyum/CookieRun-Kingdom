using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public abstract class BTElementBase
{
    public string Name { get; protected set; }
    public string GetDebugText(int indentLevel = 0)
    {
        StringBuilder debugTextBuilder = new StringBuilder();

        GetDebugTextInternal(debugTextBuilder, indentLevel);

        return debugTextBuilder.ToString();
    }

    protected abstract void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0);
}
