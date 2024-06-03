using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepresentKingdomUI : BaseUI
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _okButton;

    [SerializeField] private Sprite[] _allPortraitSprite;

    [SerializeField] private RectTransform _portraitParent;
    [SerializeField] private PortraitSlot _portraitSlotPrefab;

    private int _currentIndex = 0;
    private Image _portraitImage;
    private PortraitSlot[] _slots;

    private KingdomManager _manager;

    public override void Hide()
    {
        base.Hide();

        _manager.IsMoveCamera = true;
    }

    public void InitImage(Image portraitImage)
    {
        _portraitImage = portraitImage;
        _portraitImage.sprite = _allPortraitSprite[GameManager.Game.KingdomIndex];
    }

    public override void Init()
    {
        base.Init();

        _manager = FindObjectOfType<KingdomManager>();

        _exitButton.onClick.AddListener(() => GameManager.UI.ExitPopUpUI());
        _okButton.onClick.AddListener(() =>
        {
            GameManager.Game.KingdomIndex = _currentIndex;
            _portraitImage.sprite = _allPortraitSprite[_currentIndex];
            GameManager.UI.ExitPopUpUI();
        });

        _slots = new PortraitSlot[_allPortraitSprite.Length];

        for (int i = 0; i < _allPortraitSprite.Length; i++)
        {
            PortraitSlot slot = Instantiate(_portraitSlotPrefab, _portraitParent);
            slot.Init(_allPortraitSprite[i]);
            int index = i;
            _slots[index] = slot;
            slot.onClickButton += (() =>
            {
                for (int j = 0; j < _allPortraitSprite.Length; j++)
                    _slots[j].SetActive(false);
                _currentIndex = index;
                _slots[_currentIndex].SetActive(true);
            });
        }

        _portraitParent.sizeDelta = new Vector2(_portraitParent.sizeDelta.x, 180 * (_allPortraitSprite.Length / 4));
    }

    public override void Show()
    {
        base.Show();

        _manager.IsMoveCamera = false;

        for (int i = 0; i < _allPortraitSprite.Length; i++)
        {
            if (i == GameManager.Game.KingdomIndex)
                _slots[i].SetActive(true);
            else
                _slots[i].SetActive(false);
        }
    }
}
