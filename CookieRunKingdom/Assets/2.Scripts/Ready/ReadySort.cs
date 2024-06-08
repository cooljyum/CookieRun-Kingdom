using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySort : MonoBehaviour
{
    public List<StandingCharacter> Characters;
    private GameObject _positions;
    private List<Transform> _detailPositions;
    private void Start()
    {
        Characters = new List<StandingCharacter>();

        for(int i = 0; i < transform.childCount; i++)
        {
            Characters[i] = transform.GetChild(i).GetComponent<StandingCharacter>();
        }

        _positions = GameObject.Find("BattleReady");

        _detailPositions[0] = _positions.transform.GetChild(0).transform.GetChild(0);
        _detailPositions[1] = _positions.transform.GetChild(0).transform.GetChild(1);
        _detailPositions[2] = _positions.transform.GetChild(0).transform.GetChild(2);

    }

    private void Update()
    {
        
    }

    private void AutoSort()
    {
        if (transform.childCount < 0) return;

        if(transform.childCount == 1)
        {
            Characters[0].SetTransform(_detailPositions[1]);
        }
        else if(transform.childCount == 2) 
        {
            Characters[0].SetTransform(_detailPositions[0]);
            Characters[1].SetTransform(_detailPositions[2]);
        }
        else if(transform.childCount > 2) 
        {
            Characters[0].SetTransform(_detailPositions[0]);
            Characters[1].SetTransform(_detailPositions[2]);
            Characters[2].transform.gameObject.SetActive(false);
        }
    }
}
