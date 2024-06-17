using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

    [SerializeField]
    private GameObject _skillBtnPrefab; // 버튼 프리팹
    [SerializeField]
    private Transform panel; // Panel 오브젝트의 Transform

    private void Awake()
    {
        Instance = this;
        _skillBtnPrefab = Resources.Load<GameObject>("Prefabs/Battle/SkillBtn");
    }

    public GameObject AddSkillBtn()
    {
        GameObject skillBtn = Instantiate(_skillBtnPrefab, panel);
        return skillBtn;
    }
}
