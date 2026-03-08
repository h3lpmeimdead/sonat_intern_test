using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBox : MonoBehaviour
{
    [SerializeField] protected Canvas mainCanvas;
    
    protected virtual void Awake()
    {
        mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        mainCanvas.worldCamera = Camera.main;
    }
}
