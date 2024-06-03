using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveData
{
    // 재화
    public int money, dia, jelly, maxJelly;

    // 제작시간
    public string prevCraftTime; 

    public List<CookieInfo> allCookies;
    public List<BuildingInfo> ownedCraftableBuildings;

    public SaveData()
    {
        // 처음 하면 주어지는 재화를 정해주자
        money = 0;
        dia = 0;
        maxJelly = 40;
        jelly = maxJelly;

        prevCraftTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        allCookies = new List<CookieInfo>();
        ownedCraftableBuildings = new List<BuildingInfo>();

        // 처음하면 주어지는 쿠키들과 건물들은 정해주자
        // 처음하면 주어지는 쿠키는
        // 0 : 용감한 쿠키
        // 1 : 딸기맛 쿠키
        // 2 : 마법사맛 쿠키
        // 6 : 칠리맛 쿠키
        // 8 : 커스터드 3세맛 쿠키

        for (int i = 0; i < 10; i++)
        {
            allCookies.Add(new CookieInfo(i, false, false));
        }

        allCookies[0] = new CookieInfo(0, true, true, 0);
        allCookies[1] = new CookieInfo(1, true, true, 1);
        allCookies[2] = new CookieInfo(2, true, true, 3);
        allCookies[6] = new CookieInfo(6, true, true, 4);
        allCookies[8] = new CookieInfo(8, true, true, 8);


        // 처음하면 주어지는 건물은
        // 0 : 나무꾼의 집
        // 1 : 젤리빈 농장
        // 2 : 각설탕 채석장
        ownedCraftableBuildings.Add(new BuildingInfo(0, 4, true));
        ownedCraftableBuildings.Add(new BuildingInfo(1, 4, true));
        ownedCraftableBuildings.Add(new BuildingInfo(2, 4, true));

        for(int i = 3; i <= 19; i++)
        {
            ownedCraftableBuildings.Add(new BuildingInfo(i, 4, true));
        }


        // 처음하면 주어지는 건물들
        ownedCraftableBuildings.Add(new BuildingInfo(20, 0, true));  // 쿠키하우스
    }
}

[System.Serializable]
public class CookieInfo
{
    // 쿠키의 인덱스
    public int cookieIndex;
    
    // 능력치
    public int cookieLevel;
    public int evolutionCount;
    public int evolutionGauge;
    public int evolutionMaxGauge;

    // 상태
    public Vector3 lastKingdomPosition;
    public int battlePosition;
    public bool isBattleMember;
    public bool isHave;

    public CookieInfo(int cookieIndex, bool isBattleMember, bool isHave, int battlePosition = -1)
    {
        this.cookieIndex = cookieIndex;
        this.isBattleMember = isBattleMember;
        this.isHave = isHave;
        this.battlePosition = battlePosition;

        cookieLevel = 1;
        evolutionCount = 0;
        evolutionGauge = 0;
        evolutionMaxGauge = 20;
    }
}

/// <summary>
/// dateTime <-> string
/// https://boxwitch.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0%EC%97%90%EC%84%9C-DateTime-%EC%8B%9C%EA%B0%84%EC%9D%84-string%EC%9C%BC%EB%A1%9C-%EB%B3%80%ED%99%98-%EB%B9%84%EA%B5%90
/// </summary>
[System.Serializable]
public class BuildingInfo
{
    // 건물 인덱스
    public int buildingIndex;

    // 제작 슬롯 개수
    public int slotCount;

    // 제작 현황
    public List<CraftingItemData> craftingItemData = new List<CraftingItemData>();

    // 일하는 쿠키의 인덱스
    public int cookieWorkerIndex;

    // 제작할 수 있는가?
    public bool isCraftable;

    // 설치가 되었는가?
    public bool isInstall;

    // 설치된 장소
    public Vector3 installationPosition;

    // 마지막으로 확인된 제작시간
    public string lastTime;

    public bool isFlip;

    public BuildingInfo(int buildingIndex, int slotCount, bool isCraftable)
    {
        this.buildingIndex = buildingIndex;
        this.slotCount = slotCount;
        this.isCraftable = isCraftable;

        this.installationPosition = Vector3.zero;
        this.isInstall = false;
        this.cookieWorkerIndex = -1;
        this.craftingItemData = new List<CraftingItemData>();
        this.isFlip = false;
        for (int i = 0; i < slotCount; i++)
        {
            this.craftingItemData.Add(new CraftingItemData(ECraftingState.empty, null));
        }
    }
}
