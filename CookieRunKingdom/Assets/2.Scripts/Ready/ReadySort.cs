using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySort : MonoBehaviour
{
    //public List<StandingCharacter> Characters;
    //private GameObject _positions;
    
    private List<Transform> _detailPositions = new List<Transform>();
    private List<GameObject> _characters = new List<GameObject>();
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            _detailPositions.Add(transform.GetChild(i));
        }
    }
    
    public void Add(GameObject character)
    {
        if (_characters.Count == 0)
        {
            character.transform.position = _detailPositions[0].position;
            _characters.Add(character);            
            return;
        }

        if(_characters.Count == 1)
        {
            _characters[0].transform.position = _detailPositions[1].position;
            character.transform.position = _detailPositions[2].position;

            _characters.Add(character);            
            return;
        }

        //숫서비교해서 
        
    }

    public void Remove(GameObject character)
    {
        _characters.Remove(character);        
    }

    public int GetSize() {  return _characters.Count; }
}
