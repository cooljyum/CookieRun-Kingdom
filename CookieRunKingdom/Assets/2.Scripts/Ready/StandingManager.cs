using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingManager : MonoBehaviour
{
    protected List<List<Transform>> positions;

    [SerializeField]
    private List<GameObject> characters;

    private void Start()
    {
        positions = new List<List<Transform>>();

        for(int i = 0; i < transform.childCount; i++)
        {
            characters.Add(transform.GetChild(i).gameObject);
        }

    }
    
}
