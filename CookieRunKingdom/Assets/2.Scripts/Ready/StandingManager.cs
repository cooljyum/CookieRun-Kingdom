using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingManager : MonoBehaviour
{    
    [SerializeField]
    private List<GameObject> characters;
    
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            characters.Add(transform.GetChild(i).gameObject);            
        }
    }    
}
