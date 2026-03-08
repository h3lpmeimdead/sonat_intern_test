using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BoxMainHomeSetting : BaseBox
{
    private static BoxMainHomeSetting instance;

    public static BoxMainHomeSetting SetUp()
    {
        if (instance == null)
            instance = Instantiate(Resources.Load<BoxMainHomeSetting>(PathPrefabs.BOX_MAINHOMESETTINGS));
        
        instance.gameObject.SetActive(true);
        return instance;
        
    }
    
    [SerializeField] private RectTransform mainBox;
    [SerializeField] private RectTransform textHeader;
    
    [Space]
    [SerializeField] private Button buttonX;
    [SerializeField] private Button buttonToggleMusic;
    [SerializeField] private Button buttonToggleSFx;
    
    [Space]
    [SerializeField] private ToggleButton toggleMusic;
    [SerializeField] private ToggleButton toggleSFx;
    
    private const string KEY_MUSIC = "KEY_MUSIC";
    private const string KEY_SFX = "KEY_SFX";
    
    protected override void Awake()
    {
        base.Awake();
        buttonX.onClick.AddListener(Close);
        buttonToggleMusic.onClick.AddListener(ToggleMusic);
        buttonToggleSFx.onClick.AddListener(ToggleSFx);
    }
    
    public void Init()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        
        mainBox.localScale = Vector2.zero;
        textHeader.localScale = Vector2.zero;
        
        sequence.Append(mainBox.DOScale(10, 0.25f).SetEase(Ease.OutBack));
        sequence.Append(textHeader.DOScale(10, 0.25f).SetEase(Ease.OutBack));
        
        bool valueMusic = PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1 ? true : false;
        bool valueSfx = PlayerPrefs.GetInt(KEY_SFX, 1) == 1 ? true : false;
        
        toggleMusic.OnInit(valueMusic);
        toggleSFx.OnInit(valueSfx);
        
        AudioManager.Instance.ApplyHighPass();
    }
    
    void Close()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        
        sequence.Append(mainBox.DOScale(0, 0.25f).SetEase(Ease.InBack));
        sequence.Append(textHeader.DOScale(0, 0.25f).SetEase(Ease.InBack));
        sequence.AppendCallback(() => gameObject.SetActive(false));
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
        AudioManager.Instance.ResetFilters();
    }

    void ToggleMusic()
    {
        bool targetValue = !(PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1);
        toggleMusic.Toggle(targetValue);

        AudioManager.Instance.SetMusic(targetValue);
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
    }

    void ToggleSFx()
    {
        bool targetValue = !(PlayerPrefs.GetInt(KEY_SFX, 1) == 1);
        toggleSFx.Toggle(targetValue);

        AudioManager.Instance.SetSFX(targetValue);
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
    }


    void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
