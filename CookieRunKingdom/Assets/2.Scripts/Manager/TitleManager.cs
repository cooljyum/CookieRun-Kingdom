using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnClickTitleBtn()
    {
        Debug.Log("TitleBtn Click");
        SceneManager.LoadScene("KingdomScene");
    }

}
