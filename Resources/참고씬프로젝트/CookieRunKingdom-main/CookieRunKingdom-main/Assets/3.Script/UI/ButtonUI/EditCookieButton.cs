using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class EditCookieButton : MonoBehaviour
{
    private enum EState { unSelected, selected, Size}

    public System.Action<BaseController> onClickButton = null;

    private CookieController _cookie;
    private CookieData _data;

    [Header("정보")]
    [SerializeField] private Button _editCookieButton;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private Image _typeImage;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private RectTransform _starParent;
    [SerializeField] private RectTransform[] _stars;

    [Header("상태")]
    [SerializeField] private GameObject _selectedImage;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _offButton;

    private CookieSelectUI _cookieSelectUI;
    
    private int _currentIndex;
    private int CurrentIndex
    {
        get
        {
            return _currentIndex;
        }
        set
        {
            _currentIndex = value;

            if (_currentIndex == (int)EState.Size)
                _currentIndex = 0;
        }
    }

    private void Awake()
    {
        _cookieSelectUI = FindObjectOfType<CookieSelectUI>();
    }

    public void UpdateUI(CookieController cookie)
    {
        _cookie = cookie;
        _data = (CookieData)(_cookie.Data);

        _portraitImage.sprite = _data.IdleSprite;
        _typeImage.sprite = _data.TypeSprite;
        _levelText.text = _cookie.CookieStat.CookieLevel.ToString();

        // 중복되있는 쿠키라면
        foreach(BaseController selectedCookie in _cookieSelectUI.SelectedTempCookies)
            if(selectedCookie.Data.CharacterName == _data.CharacterName)
            {
                CurrentIndex = 1;
                _selectedImage.SetActive(true);
            }

        _editCookieButton.onClick.RemoveAllListeners();
        _editCookieButton.onClick.AddListener(() => OnClickEditCookieButton(CurrentIndex++));

        // 별 동기화
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].gameObject.SetActive(false);
        for (int i = 0; i < _cookie.CookieStat.EvolutionCount; i++)
            _stars[i].gameObject.SetActive(true);
        _starParent.anchoredPosition = -(Vector3.right * _stars[0].sizeDelta.x * _cookie.CookieStat.EvolutionCount * 0.5f);
    }

    public void InitUI()
    {
        CurrentIndex = 1;
        OnClickEditCookieButton(CurrentIndex++);
    }

    private void OnClickEditCookieButton(int index)
    {
        InitButton();

        EState state = (EState)index; 

        switch (state)
        {
            case EState.unSelected:
                OnClickUnSelected();
                break;
            case EState.selected:
                OnClickOff();
                break;
/*            case EState.info:
                OnClickInfo();
                break;
            case EState.off:
                OnClickOff();
                break; */
        }
    }

    private void InitButton()
    {
        _selectedImage.SetActive(false);
        _infoButton.gameObject.SetActive(false);
        _offButton.gameObject.SetActive(false);
    }

    private void OnClickUnSelected()
    {
        // 이거 누르면 체크 그림 보여주고
        // cookieSelectUI에 올린다.
        _selectedImage.SetActive(true);
        _cookieSelectUI.AddCookie(_cookie);
    }

    private void OnClickSelected()
    {
        // 체크 그림 없애고 초록색 정보보기 버튼 보여주고
        // cookieSelectUI에 있는 쿠키에 화살표가 보여주고 해제하기 버튼 보여준다.

        _infoButton.gameObject.SetActive(true);
    }

    private void OnClickInfo()
    {
        // 정보보기 버튼 없애고 빨간색 해제 버튼 보여주고
        // 정보 보여준다.

        // 화살표는 그대로 => 함수로 빼는게 좋을듯

        _offButton.gameObject.SetActive(true);
    }

    private void OnClickOff()
    {
        // 누르면 버튼은 처음 상태로 돌아가고
        // cookieSelectUI에 해제한다.

        _cookieSelectUI.RemoveCookie(_cookie);
    }
}
