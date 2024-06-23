using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{ 
    //Btn
    private Button _btn; // 버튼 참조

    //BtnImg
    private Sprite _onImg; // 활성화 이미지
    private Sprite _offImg; // 비활성화 이미지
    private Sprite _skillImg; // 스킬 이미지

    private Image _waitBgImg; // Wait 상태일때 채워지는 이미지

    [Header("Btn State")]
    [SerializeField]
    private bool _isSkill = false; //스킬을 사용할 수 있는 상태인지
    [SerializeField]
    private SkillBtnState _curState; // 현재 버튼 상태
    public SkillBtnState CurState 
    {
        get { return _curState; }
    }

    [Header("Btn Child Obj")]
    [SerializeField]
    private Material _outlineMat;
    [SerializeField]
    private GameObject _particle;
    [SerializeField]
    private GameObject _hpBar;

    [Header("Btn Owner Obj")]
    [SerializeField]
    private GameObject _owner;


    public enum SkillBtnState
    {
        On,
        Off,
        Wait,
        Ready,
        Skill
    }

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _waitBgImg = transform.GetChild(0).GetComponent<Image>();
        _particle = transform.GetChild(1).gameObject;
        _hpBar = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        SetState(SkillBtnState.On);

        _btn.onClick.AddListener(OnSkillBtnClick);
    }

    public void SetOwner(GameObject owner) 
    {
        _owner = owner;
    }

    public void SetImg(SkillBtnImg skillBtnImg)
    {
        _onImg = skillBtnImg.OnImg;
        _offImg = skillBtnImg.OffImg;
        _skillImg = skillBtnImg.SkillImg;
    }

    public void SetState(SkillBtnState newState)
    {
        if (_curState == newState) return;

        _curState = newState;

        switch (_curState)
        {
            #region
            case SkillBtnState.On:
                _isSkill = false;
                _btn.image.sprite = _onImg;
                _btn.image.material = null;
                _particle.SetActive(false);
                _hpBar.SetActive(false);
                break;
            case SkillBtnState.Off:
                _isSkill = false;
                _btn.image.sprite = _offImg;
                _btn.image.material = null;
                _particle.SetActive(false);
                _hpBar.SetActive(false);
                _btn.interactable = false;
                break;
            case SkillBtnState.Wait:
                _isSkill = false;
                _btn.image.material = null;
                _particle.SetActive(false);
                _hpBar.SetActive(true);
                break;
            case SkillBtnState.Ready:
                _isSkill = true;
                _btn.image.material = _outlineMat;
                _particle.SetActive(true);
                _hpBar.SetActive(true);
                break;
            case SkillBtnState.Skill:
                _isSkill = false;
                _btn.image.sprite = _skillImg;
                _btn.image.material = null;
                _particle.SetActive(false);
                _hpBar.SetActive(false);
                break;
            #endregion
        }
    }

    public void OnSkillBtnClick()
    {
        if (!_isSkill) return;

        print("SkillBtn Click!");
       
        if (_curState == SkillBtnState.Ready && !BattleUIManager.Instance.IsSkillEffect)
        {
            _owner.GetComponent<BattleCookie>().Skill();
            SetState(SkillBtnState.Skill);
            BattleUIManager.Instance.IsSkillEffect = true;
        }
    }

    public void SetWaitBgFillAmount(float amount)
    {
        if (_waitBgImg != null)
        {
            _waitBgImg.fillAmount = Mathf.Clamp01(amount);
            if (amount <= 0)
            {
                _isSkill = true;
            }
        }
    }

    public void SetHPBarUI(float value) 
    {
        if (_hpBar != null) 
        {
            _hpBar.GetComponent<Slider>().value = value;

            if (value <= 0)
                SetState(SkillBtnState.Off);
        }
    }
}
