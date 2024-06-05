using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckSettingBtn : MonoBehaviour
{
    private CharacterData _characterData;

    private Image _profileImage;

    private void Awake()
    {
        _profileImage = GetComponent<Image>();
    }

    public void SetData(CharacterData characterData)
    {
        _profileImage.sprite = characterData.profileImage;
    }
}
