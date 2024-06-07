using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingCharacter : MonoBehaviour
{
    private CharacterData _characterData;

    [SerializeField]
    private Transform _transform;

    private void Start()
    {
        transform.position = _transform.position;
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetData(CharacterData characterData)
    {
        _characterData = characterData;
    }
}
