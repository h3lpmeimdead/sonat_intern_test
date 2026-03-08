using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BoxWin : BaseBox
{
    private static BoxWin instance;

    public static BoxWin SetUp()
    {
        if (instance == null)
            instance = Instantiate(Resources.Load<BoxWin>(PathPrefabs.BOX_WIN));
        
        instance.gameObject.SetActive(true);
        return instance;
        
    }
    
    [SerializeField] private RectTransform winText;
    
    [SerializeField] private Button buttonResume;
    [SerializeField] private Button buttonRetry;
    [SerializeField] private Button buttonHome;
    
    [SerializeField] private CinemachineImpulseSource homeSource;

    [Space] 
    [SerializeField] private List<GameObject> fx = new List<GameObject>(); 
    
    protected override void Awake()
    {
        base.Awake();
        buttonResume.onClick.AddListener(Resume);
        buttonRetry.onClick.AddListener(Retry);
        buttonHome.onClick.AddListener(Home);
    }

    public void Init()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        
        winText.localScale = Vector2.zero;
        
        sequence.Append(winText.DOScale(5, 0.25f).SetEase(Ease.OutBack));
        AudioManager.Instance.ApplyHighPass();
        AudioManager.Instance.PlaySFX(AudioPaths.WIN, loop: false, isPitchShift: true);
        
        for (int i = 0; i < fx.Count; i++)
        {
            if (fx[i] != null)
            {
                GameObject spawnedFx = Instantiate(fx[i], transform);
                spawnedFx.SetActive(true);
                
                ParticleSystem ps = spawnedFx.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    Destroy(spawnedFx, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }
        }
    }

    public void OnDespawn()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        
        sequence.Append(winText.DOScale(0, 0.25f).SetEase(Ease.InOutBack));
        sequence.AppendCallback(() => gameObject.SetActive(false));
        CameraShake.Instance.GloabalCameraShake(homeSource);
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
        AudioManager.Instance.ResetFilters();
    }

    void Resume()
    {
        ActiveLevelController.activeLevel++;
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("activeLevel", ActiveLevelController.activeLevel);
        OnDespawn();
    }

    void Retry()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("activeLevel", ActiveLevelController.activeLevel);
        OnDespawn();
    }
    
    void Home()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        sequence.AppendCallback(() => gameObject.SetActive(false));
        sequence.Append(winText.DOScale(0, 0.25f).SetEase(Ease.InOutBack));
        sequence.AppendCallback(() => Transitioner.Instance.TransitionToScene(SceneNames.MENU));
        
        CameraShake.Instance.GloabalCameraShake(homeSource);
        AudioManager.Instance.PlaySFX(AudioPaths.CLICK, loop: false, isPitchShift: true);
    }
}
