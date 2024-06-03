using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeSequence : BTNodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return false;
    }
    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }
    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[_children.Count - 1].LastStatus;
    }
}
