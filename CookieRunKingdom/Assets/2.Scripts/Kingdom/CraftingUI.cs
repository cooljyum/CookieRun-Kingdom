using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{

    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(CraftItemInfo info)
    {
        TimeManager.Instance.AddTime(0, info);
    }
    
    // Update is called once per frame
    void Update()
    {
        //float time = TimeManager.Instance.GetRemainTime()
    }
}
