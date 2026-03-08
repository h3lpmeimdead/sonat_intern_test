using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        BoxMainHome.SetUp().Init();
    }
}

public static class SceneNames
{
    public const string MENU = "MainMenu";
    public const string GAME = "Game";
}
