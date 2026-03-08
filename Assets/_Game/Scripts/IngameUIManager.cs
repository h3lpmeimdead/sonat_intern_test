using System;
using Cinemachine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    [Space]
    [SerializeField] private Button buttonSetting;
    [SerializeField] private Button buttonAddBottle;
    [SerializeField] private Button buttonUndo;
    [SerializeField] private Button buttonShuffle;
    [SerializeField] private Button buttonSkipLevel;

    private void Awake()
    {
        buttonSetting.onClick.AddListener(OpenSettings);
        buttonUndo.onClick.AddListener(Undo);
        buttonAddBottle.onClick.AddListener(AddBottle);
        buttonShuffle.onClick.AddListener(Shuffle);
        buttonSkipLevel.onClick.AddListener(SkipLevel);
    }

    private void Update()
    {
        
    }

    void OpenSettings()
    {
        BoxSetting.SetUp().Init();
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
    }

    private void Undo()
    {
        
    }

    private void AddBottle()
    {
        
    }
    
    public void Shuffle()
    {
        
    }

    public void SkipLevel()
    {
        BoxWin.SetUp().Init();
        ActiveLevelController.activeLevel++;
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("activeLevel", ActiveLevelController.activeLevel);
    }
}
