using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaCalculator : MonoBehaviour
{
    private KingdomManager _manager;

    public void Awake()
    {
        _manager = FindObjectOfType<KingdomManager>();
    }

    public List<Vector2> CalculateGacha(GachaData gachaData)
    {
        List<CookieController> cookies = _manager.allCookies;

        // ÀÎµ¦½º°¡ ÇØ´ç ÄíÅ°°í  (ÄíÅ° È®·ü, ¿µÈ¥¼® È®·ü)
        List<Vector2>  _gachaPercentage = new List<Vector2>();

        int epicCount = 0, rareCount = 0, commonCount = 0;
        for(int i = 0; i < cookies.Count; i++)
        {
            CookieData cookieData = (CookieData)(cookies[i].Data);

            if (cookieData.CookieGrade == ECookieGrade.Common)
                commonCount++;
            else if (cookieData.CookieGrade == ECookieGrade.Rare)
                rareCount++;
            else if (cookieData.CookieGrade == ECookieGrade.Epic)
                epicCount++;
        }

        float pickUpPercentage, epicPercentage, rarePercentage, commonPercentage;

        if(gachaData.IsPickUp)
        {
            pickUpPercentage = gachaData.EpicPercentage / 2 * gachaData.RatioBetweenCookieToSoul;
            epicPercentage = gachaData.EpicPercentage / 2 / (epicCount - 1) * gachaData.RatioBetweenCookieToSoul;
            rarePercentage = gachaData.RarePercentage / rareCount * gachaData.RatioBetweenCookieToSoul;
            commonPercentage = gachaData.CommonPercentage / commonCount * gachaData.RatioBetweenCookieToSoul;
        }
        else
        {
            pickUpPercentage = 0;
            epicPercentage = gachaData.EpicPercentage / epicCount * gachaData.RatioBetweenCookieToSoul;
            rarePercentage = gachaData.RarePercentage / rareCount * gachaData.RatioBetweenCookieToSoul;
            commonPercentage = gachaData.CommonPercentage / commonCount * gachaData.RatioBetweenCookieToSoul;
        }

        float soulAmount = (1 - gachaData.RatioBetweenCookieToSoul) / gachaData.RatioBetweenCookieToSoul;

        for(int i = 0; i < cookies.Count; i++)
        {
            if (gachaData.IsPickUp && i == gachaData.PickUpCookie)
            {
                _gachaPercentage.Add(new Vector2(pickUpPercentage, pickUpPercentage * soulAmount));
            }
            else
            {
                CookieData cookieData = (CookieData)(cookies[i].Data);
                if (cookieData.CookieGrade == ECookieGrade.Common)
                    _gachaPercentage.Add(new Vector2(commonPercentage, commonPercentage * soulAmount));
                else if (cookieData.CookieGrade == ECookieGrade.Rare)
                    _gachaPercentage.Add(new Vector2(rarePercentage, rarePercentage * soulAmount));
                else if (cookieData.CookieGrade == ECookieGrade.Epic)
                    _gachaPercentage.Add(new Vector2(epicPercentage, epicPercentage * soulAmount));
            }
        }

        return _gachaPercentage;
    }
}
