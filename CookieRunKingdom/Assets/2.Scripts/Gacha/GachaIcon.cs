using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GachaIcon : MonoBehaviour
{ 
    private CharacterData _characterData;
    public CharacterData CharacterData { get { return _characterData; } }
    private SkeletonGraphic _skeletonGraphic;
    private TextMeshProUGUI _nameText;
    private void Awake()
    {
        _skeletonGraphic = GetComponent<SkeletonGraphic>();
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetData(int key)
    {
        _characterData = DataManager.Instance.GetCharacterData(key);
        _skeletonGraphic.skeletonDataAsset = _characterData.SkeletonDataAsset;
        _nameText.text = _characterData.Name;
    }
}
