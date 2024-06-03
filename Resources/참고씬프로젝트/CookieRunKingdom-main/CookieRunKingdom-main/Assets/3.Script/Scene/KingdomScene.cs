using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomScene : BaseScene
{
    [Header("몰라")]
    [SerializeField] private GroundGenerator _groundGenerator;

    [Header("부모 Transform")]
    [SerializeField] private Transform _buildingParent;
    [SerializeField] private Transform _cookieParent;

    [SerializeField] private KingdomManager _kingdomManager;

    private Camera _camera;

    protected override void Init()
    {
        // 땅 만들기
        _groundGenerator.Generate();

        // 생성할건 다 생성하고...
        BuildingPreviewTileObjectPool.instance.Init();

        // 왕국 배치
        ArrangeBuilding();

        // 쿠키 생성
        ArrangeCookies();

        // 건물들 제작
        CraftItem();

        // 왕국 상태패턴
        _kingdomManager.Init();

        // 로딩창 풀어준다.
        GameManager.Scene.EndLoading();
        GameManager.Sound.PlayBgm(EBGM.lobby);

        _camera = Camera.main;
        _camera.transform.position = new Vector3(4.6f, 9f, -10f);
        _camera.orthographicSize = 13;
    }

    // 왕국배치
    private void ArrangeBuilding()
    {
        BuildingController[] allBuildingsData = DataBaseManager.Instance.AllBuildings;
        List<BuildingInfo> ownedBuildings = GameManager.Game.OwnedCraftableBuildings;

        for(int i = 0; i < ownedBuildings.Count; i++)
        {
            BuildingInfo buildingInfo = ownedBuildings[i];
            BuildingController building = Instantiate(allBuildingsData[buildingInfo.buildingIndex], _buildingParent);
            
            building.BuildingWorker.LoadBuilding();

            if(buildingInfo.isInstall)
            {
                building.gameObject.SetActive(true);
                _kingdomManager.buildingsInKingdom.Add(building);
            }
            else
            {
                building.gameObject.SetActive(false);
                _kingdomManager.buildingsInInventory.Add(building);
            }
        }
        // GridManager.Instance.buildingGridData.VisualizeGridMapData();
    }

    // 쿠키 생성
    private void ArrangeCookies()
    {
        CookieController[] allCookiesData = DataBaseManager.Instance.AllCookies;
        List<CookieInfo> ownedCookies = GameManager.Game.allCookies;

        for(int i = 0; i < ownedCookies.Count; i++)
        {
            CookieInfo cookieInfo = ownedCookies[i];

            CookieController cookie = Instantiate(allCookiesData[cookieInfo.cookieIndex], _cookieParent);

            // 정보 넣기
            cookie.CookieStat.LoadCookie();

            if (cookieInfo.lastKingdomPosition == Vector3.zero)
                cookie.transform.position = GridManager.Instance.ReturnEmptyTilePosition();
            else
                cookie.transform.position = cookieInfo.lastKingdomPosition;

            _kingdomManager.allCookies.Add(cookie);

            if(cookie.CookieStat.IsHave)
            {
                cookie.CookieCitizeon.StartKingdomAI();
            }
            else
            {
                cookie.gameObject.SetActive(false);
            }
        }

        // FindObjectOfType<BTDebugUI>()._tree = _kingdomManager.allCookies[0].CookieCitizeon;
    }

    // 건물들 제작
    private void CraftItem()
    {
        List<BuildingController> buildingsInKingdom = _kingdomManager.buildingsInKingdom;
        List<BuildingInfo> ownedBuildings = GameManager.Game.OwnedCraftableBuildings;
        int buildingCount = 0;

        for (int i = 0; i < ownedBuildings.Count; i++)
        {
            if (!ownedBuildings[i].isCraftable)
                continue;

            BuildingInfo buildingInfo = ownedBuildings[i];
            if(buildingInfo.isInstall)
            {
                BuildingController building = buildingsInKingdom[buildingCount];
                if (buildingInfo.cookieWorkerIndex != -1)
                {
                    // 일꾼설정
                    foreach (CookieController cookie in _kingdomManager.allCookies)
                        if (((CookieData)cookie.Data).CookieIndex == buildingInfo.cookieWorkerIndex)
                            building.BuildingWorker.LoadWorker(cookie);
                }

                buildingCount++;
            }
        }
    }
}
