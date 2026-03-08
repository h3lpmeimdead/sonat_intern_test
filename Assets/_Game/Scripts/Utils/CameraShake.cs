using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField] private float _globalShakeForce = 0.03f;

    public void GloabalCameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(_globalShakeForce);
    }
}