using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{ 
    //Btn
    private Button _btn; // ��ư ����

    //BtnImg
    private Sprite _onImg; // Ȱ��ȭ �̹���
    private Sprite _offImg; // ��Ȱ��ȭ �̹���
    private Sprite _skillImg; // ��ų �̹���

    private Image _waitBgImg; // Wait �����϶� ä������ �̹���

    [Header("Btn State")]
    [SerializeField]
    private bool _isSkill = false; //��ų�� ����� �� �ִ� ��������
    [SerializeField]
    private SkillBtnState _curState; // ���� ��ư ����

    [Header("Btn Child Obj")]
    [SerializeField]
    private Material _outlineMat;
    [SerializeField]
    private GameObject _particle;
    [SerializeField]
    private GameObject _hpBar;


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
            case SkillBtnState.On:
                _isSkill = false;
                _btn.image.sprite = _onImg;
                break;
            case SkillBtnState.Off:
                _btn.image.sprite = _offImg;
                break;
            case SkillBtnState.Wait:
                _hpBar.SetActive(true);
                break;
            case SkillBtnState.Ready:
                _isSkill = true;
                _btn.image.material = _outlineMat;
                _particle.SetActive(true);
                break;
            case SkillBtnState.Skill:
                _btn.image.sprite = _skillImg;
                _btn.image.material = null;
                _particle.SetActive(false);
                _hpBar.SetActive(false);
                break;
        }
    }

    public void OnSkillBtnClick()
    {
        if (!_isSkill) return;

        print("SkillBtn Click!");
       
        if (_curState == SkillBtnState.Ready)
        {

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
        }
    }
}
