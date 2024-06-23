using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySort : MonoBehaviour
{
    //캐릭터를 배치할 위치들을 저장하는 리스트
    private List<Transform> _detailPositions = new List<Transform>();
    public List<Transform> DetailPositions
    {  
        get { return _detailPositions; } 
    } 

    //현재 배치된 캐릭터들을 저장하는 리스트
    private List<GameObject> _characters = new List<GameObject>();
    public List<GameObject> Characters
    {
        get { return _characters; }
    }

    private void Awake()
    {
        //해당 포지션에 해당하는 세부 위치 트랜스 폼을 리스트에 추가
        for (int i = 0; i < 3; i++)
        {
            _detailPositions.Add(transform.GetChild(i));
        }
    }
    
    //캐릭터를 추가하는 함수
    public void Add(GameObject character)
    {
        //리스트가 비어있을 때 0번(중앙)위치에 배치
        if (_characters.Count == 0)
        {
            character.transform.position = _detailPositions[0].position;
            _characters.Add(character);            
            return;
        }

        //리스트에 하나의 캐릭터가 있을 때 기존 캐릭터를 1번(하단)으로 옮기고,
        //새로운 캐릭터를 2번(상단)에 추가
        if(_characters.Count == 1)
        {
            _characters[0].transform.position = _detailPositions[1].position;
            character.transform.position = _detailPositions[2].position;

            _characters.Add(character);            
            return;
        }
    }

    //캐릭터를 제거하는 함수
    public void Remove(GameObject character)
    {
        //리스트에 두 개 이상의 캐릭터가 있을 때,
        //제거 대상 캐릭터 삭제 후 남는 캐릭터는 0번(중앙)으로 이동
        if (_characters.Count > 1)
        {
            _characters.Remove(character);
            _characters[0].transform.position = _detailPositions[0].position;
        }
        _characters.Remove(character);      
    }

    //특정 캐릭터가 리스트에 포함되어 있는지 확인하는 함수
    public bool Contains(GameObject character)
    {
        return _characters.Contains(character);
    }
    
    //리스트를 비우는 함수
    public void Clear()
    {
        _characters.Clear();
    }

    //리스트의 크기를 반환하는 함수
    public int GetSize() {  return _characters.Count; }
}
