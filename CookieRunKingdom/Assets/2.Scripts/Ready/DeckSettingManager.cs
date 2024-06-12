using TMPro;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{
    [SerializeField]
    private Transform _deckBtnContent;

    private CharacterData _characterData;
    private GameObject _teamPower;
    private TextMeshProUGUI _teamPowerText;
   

    private void Start()
    {
        SetDeckBtn();
        _teamPower = GameObject.Find("TeamPower");
        _teamPowerText = _teamPower.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        SetStandingCharacters();        
        
    }

    private void SetStandingCharacters()
    {

    }
    private void SetDeckBtn()
    {
        GameObject btnPrefab = Resources.Load<GameObject>("Prefabs/BattleReady/DeckBtn");

        foreach(int myCard in GameManager.Instance.MyCards )
        {
            CharacterData data = DataManager.Instance.GetCharacterData(myCard);

            GameObject btnObj = Instantiate(btnPrefab, _deckBtnContent);
            btnObj.GetComponent<DeckSettingBtn>().SetData(data);
        }
    }
}
