using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeAction : BTNodeBase
{
    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[_children.Count - 1].LastStatus;
    }

}
