using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSO afterSceneLoaderEvent;

    private CinemachineConfiner2D Confiner2D;
    public CinemachineCollisionImpulseSource impulseSource;
    public VoidEventSO CameraShakeEvent;
    private void Awake()
    {
        Confiner2D = GetComponent<CinemachineConfiner2D>();

    }

    private void OnEnable()
    {
        CameraShakeEvent.OnEventRaise += cameraShakeEvent;
        afterSceneLoaderEvent.OnEventRaise += OnAfterSceneLoaderEvent;
    }

    private void OnDisable()
    {
        CameraShakeEvent.OnEventRaise -= cameraShakeEvent;
        afterSceneLoaderEvent.OnEventRaise -= OnAfterSceneLoaderEvent;
    }

    private void OnAfterSceneLoaderEvent()
    {
        GetNewCameraBounds();
    }

    private void cameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    /// <summary>
    /// �����л�������������Χ
    /// </summary>

    /*private void Start()
    {
        GetNewCameraBounds();
    }*/
    public void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        else
        {
            Confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
            Confiner2D.InvalidateCache();
        }
    }
}
