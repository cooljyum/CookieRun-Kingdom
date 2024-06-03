using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GachaData : ScriptableObject
{
    [SerializeField] private bool _isPickUp = false;
    [SerializeField] private int _pickUpCookie;
    
    [SerializeField] private float _epicPercentage = 20f;
    [SerializeField] private float _rarePercentage = 35f;
    [SerializeField] private float _commonPercentage = 45f;

    [SerializeField] private float _ratioBetweenCookieToSoul = 0.25f;  // ¿ÂÀüÇÑ ÄíÅ° 1 : ÄíÅ° ¿µÈ¥¼® 4

    public bool IsPickUp => _isPickUp;
    public int PickUpCookie => _pickUpCookie;


    public float EpicPercentage => _epicPercentage;
    public float RarePercentage => _rarePercentage;
    public float CommonPercentage => _commonPercentage;

    public float RatioBetweenCookieToSoul => _ratioBetweenCookieToSoul;
}
