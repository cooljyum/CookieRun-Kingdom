using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingManager : MonoBehaviour
{
    private List<List<int>> positions;

    [SerializeField]
    private List<GameObject> characters;

    private void Start()
    {
        positions = new List<List<int>>();

        for(int i = 0; i < transform.childCount; i++)
        {
            characters.Add(transform.GetChild(i).gameObject);
        }

    }
    
}
