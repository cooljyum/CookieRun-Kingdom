using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageData[] _stageData;
    [SerializeField] private Transform[] _stagePoses;
    [SerializeField] private StageButtonUI _stageButtonPrefab;

    [SerializeField] private Transform _buttonParent;

    [SerializeField] private StageSelectUI _stageSelectUI;

    private void Awake()
    {
        // 모험 UI가 Show될 때 해야해
        Init();
    }

    public void Init()
    {
        _buttonParent.DestroyAllChild();

        for(int i = 0; i < _stageData.Length; i++)
        {
            StageButtonUI stageButton = Instantiate(_stageButtonPrefab, _buttonParent);
            stageButton.transform.position = _stagePoses[i].position;

            stageButton.Init(_stageData[i]);
            int index = i;
            stageButton.GetComponent<Button>().onClick.AddListener(() => OnClickStageButton(_stageData[index], stageButton.transform.position));

            if(_stageData[i].IsBoss)
            {
                SkeletonAnimation animation = Instantiate(_stageData[i].BossSkeletonAnimation, _stagePoses[i]);
                animation.transform.localPosition = Vector3.zero;
                animation.Initialize(true);
                animation.AnimationState.SetAnimation(0, "battle_idle", true);

                Renderer renderer = animation.GetComponent<Renderer>();
                renderer.sortingLayerName = "GridLower";
                renderer.sortingOrder = -100;
            }
        }
    }

    private void OnClickStageButton(StageData stageData, Vector3 touchPos)
    {
        // 카메라 이동
        // 쿠키 이동

        _stageSelectUI.InitStageData(stageData, touchPos);
        GameManager.UI.ShowPopUpUI(_stageSelectUI);
    }
}
