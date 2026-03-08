using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BoxLose : BaseBox
{
    private static BoxLose instance;

    public static BoxLose SetUp()
    {
        if (instance == null)
            instance = Instantiate(Resources.Load<BoxLose>(PathPrefabs.BOX_LOSE));
        
        instance.gameObject.SetActive(true);
        return instance;
        
    }
    
    [SerializeField] private RectTransform loseText;

    [Space] [SerializeField] private List<GameObject> fx = new List<GameObject>(); 
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        
        loseText.localScale = Vector2.zero;
        
        sequence.Append(loseText.DOScale(5, 0.25f).SetEase(Ease.OutBack));
        AudioManager.Instance.ApplyLowPass();
        AudioManager.Instance.PlaySFX(AudioPaths.LOSE, loop: false, isPitchShift: true);

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
        
        sequence.Append(loseText.DOScale(0, 0.25f).SetEase(Ease.InOutBack));
        sequence.AppendCallback(() => gameObject.SetActive(false));
        AudioManager.Instance.ResetFilters();
    }
}