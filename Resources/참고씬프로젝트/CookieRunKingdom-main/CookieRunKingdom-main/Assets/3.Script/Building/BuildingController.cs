using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private BuildingData _buildingData;

    protected BuildingAnimator _buildingAnimator;
    protected BuildingWorker _buildingWorker;
    protected BuildingEditor _buildingEditor;


    public BuildingData Data => _buildingData;

    public BuildingAnimator BuildingAnimator => _buildingAnimator;
    public BuildingWorker BuildingWorker => _buildingWorker;
    public BuildingEditor BuildingEditor => _buildingEditor;


    protected virtual void Awake()
    {
        _buildingAnimator = GetComponent<BuildingAnimator>();
        _buildingWorker = GetComponent<BuildingWorker>();
        _buildingEditor = GetComponent<BuildingEditor>();

        _buildingAnimator.Init(this);
        _buildingEditor.Init(this);
        _buildingWorker.Init(this);
    }
}
