using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaUnitUI : MonoBehaviour
{
    public float Percentage { get; private set; }
    public string CookieName { get; private set; }


    [SerializeField] private Image _baseImage;
    [SerializeField] private TextMeshProUGUI _cookieGradeText;
    [SerializeField] private Image _cookieImage;
    [SerializeField] private TextMeshProUGUI _cookieNameText;
    [SerializeField] private TextMeshProUGUI _cookieTypeText;
    [SerializeField] private TextMeshProUGUI _cookiePosition;
    [SerializeField] private TextMeshProUGUI _cookiePercentage;

    [SerializeField] private GameObject _pickUptext;

    public void Init(CookieData data, bool isSoul, float percentage, Color gradeColor, Color baseColor)
    {
        Percentage = percentage;
        CookieName = data.CharacterName;

        _baseImage.color = baseColor;
        _cookieGradeText.color = gradeColor;
        _cookieGradeText.text = data.CookieGradeName;

        _cookieImage.sprite = isSoul ? data.EvolutionSprite : data.CookieHeadSprite;
        _cookieNameText.text = isSoul ? data.CharacterName + "ÀÇ ¿µÈ¥¼®" : data.CharacterName;

        _cookieTypeText.text = data.CookieTypeName;
        _cookieTypeText.text = data.CookieTypeName;
        _cookiePosition.text = data.CookiePositionName;

        _cookiePercentage.text = percentage.ToString("F3") + "%";
    }

    public void ShowPickUp()
    {
        _pickUptext.SetActive(true);
    }
}
