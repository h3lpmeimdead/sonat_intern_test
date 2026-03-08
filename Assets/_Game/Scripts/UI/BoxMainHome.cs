using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;

public class BoxMainHome : BaseBox
{
    private static BoxMainHome instance;

    public static BoxMainHome SetUp()
    {
        if (instance == null)
            instance = Instantiate(Resources.Load<BoxMainHome>(PathPrefabs.BOX_MAINHOME));
        
        instance.gameObject.SetActive(true);
        return instance;
        
    }
    
    [SerializeField] private RectTransform buttonPlayRectTransform;
    [SerializeField] private RectTransform buttonSettingRectTransform;
    [SerializeField] private RectTransform buttonQuitRectTransform;
    [SerializeField] private RectTransform textTitleRectTransform;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonSetting;
    [SerializeField] private CinemachineImpulseSource playButton;
    [SerializeField] private CinemachineImpulseSource exitButton;
    [SerializeField] private CinemachineImpulseSource settingButton;

    protected override void Awake()
    {
        base.Awake();
        buttonExit.onClick.AddListener(Exit);
        buttonPlay.onClick.AddListener(Play);
        buttonSetting.onClick.AddListener(Setting);
    }

    public void Init()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        textTitleRectTransform.localScale = Vector2.zero;
        
        sequence.Append(textTitleRectTransform.DOScale(2, 1f).SetEase(Ease.OutBack));
    }
    
    private void Exit()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        float duration = 0.5f;
        float strength = 0.5f;

        sequence.Append(buttonQuitRectTransform.DOShakeScale(duration, strength).SetEase(Ease.OutBack));
        sequence.AppendCallback(() =>
        Application.Quit());
        
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
        CameraShake.Instance.GloabalCameraShake(exitButton);
    }

    private void Play()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        float duration = 0.5f;
        float strength = 0.5f;

        sequence.Append(buttonPlayRectTransform.DOShakeScale(duration, strength).SetEase(Ease.OutBack));
        sequence.AppendCallback(() =>
        {
            Transitioner.Instance.TransitionToScene("Game");
        });

        CameraShake.Instance.GloabalCameraShake(playButton);
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
    }

    private void Setting()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        float duration = 0.5f;
        float strength = 0.5f;

        sequence.Append(buttonSettingRectTransform.DOShakeScale(duration, strength).SetEase(Ease.OutBack));
        sequence.AppendCallback(() =>
        {
            BoxSetting.SetUp().Init();
        });
        
        CameraShake.Instance.GloabalCameraShake(settingButton);
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
    }
    
}
