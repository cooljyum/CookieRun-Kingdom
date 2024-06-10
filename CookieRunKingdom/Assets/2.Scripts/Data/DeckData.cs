using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Scriptable Object/DeckData", order = 1)]
public class DeckData : ScriptableObject
{
    public List<CharacterData> CharacterDatas;
    
}
