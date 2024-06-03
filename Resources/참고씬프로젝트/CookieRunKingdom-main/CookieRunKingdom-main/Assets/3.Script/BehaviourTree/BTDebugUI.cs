using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BTDebugUI : MonoBehaviour
{
    public BehaviorTree _tree;
    [SerializeField] private TextMeshProUGUI _debugText;

    private void Start()
    {
        _debugText.text = "";
    }


    private void Update()
    {
        if (_tree == null)
            return;

        _debugText.text = _tree.GetDebugText();
    }

}
