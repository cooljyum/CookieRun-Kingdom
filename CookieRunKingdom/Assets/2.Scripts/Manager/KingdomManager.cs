using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomManager : MonoBehaviour
{
    public static KingdomManager instance;

    private void Start()
    {
        instance = this;


    }
}
