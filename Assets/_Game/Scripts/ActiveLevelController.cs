using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveLevelController : MonoBehaviour
{
    public static int activeLevel;
    void Start()
    {
        if (PlayerPrefs.HasKey("activeLevel") == true)
        {
            activeLevel = PlayerPrefs.GetInt("activeLevel");
        }
        else activeLevel = 0;
    }
}