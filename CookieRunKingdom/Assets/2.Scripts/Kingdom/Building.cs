using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Building : MonoBehaviour
{
    private ScriptableObject _buildingData;
    private SkeletonAnimation _skeletonAnimation;

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void Build(BuildingData buildingData)
    {
        gameObject.SetActive(true);

        _buildingData = buildingData;
        _skeletonAnimation.AnimationName = "loop_back";
    }
}
