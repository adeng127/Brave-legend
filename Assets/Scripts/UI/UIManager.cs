using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("广播")]
    public VoidEventSO pauseEvent;

    public PlayerStar playerStar;
    [Header("事件监听")]
    public CharacterEventSO HealthEvent;
    public SceneLoadEventSO unLoadedSceneEvent;
    public VoidEventSO LoadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncVolumeEvent;
    [Header("组件")]
    public GameObject gameOverPanel;
    public GameObject restartbtn;
    public GameObject mobileTouch;

    public Button settingButton;
    public GameObject pausePanel;
    public Slider volumeSlider;
    private void Awake()
    {
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif

        settingButton.onClick.AddListener(TogglePausePanel);
    }

    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            
        }
        else
        {
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
           
        }

    }

    private void OnEnable()
    {
        HealthEvent.OnEventRaised+=OnHealthEvent;
        unLoadedSceneEvent.loadRequestEvent += OnUnLoadedEvent;
        LoadDataEvent.OnEventRaise += OnLoadDataEvent;
        gameOverEvent.OnEventRaise += OnGameOverEvent;
        backToMenuEvent.OnEventRaise += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaise += OnSyncVolumeEvent;
    }

    
    private void OnDisable()
    {
        HealthEvent.OnEventRaised -= OnHealthEvent;
        unLoadedSceneEvent.loadRequestEvent -= OnUnLoadedEvent;
        LoadDataEvent.OnEventRaise -= OnLoadDataEvent;
        gameOverEvent.OnEventRaise -= OnGameOverEvent;
        backToMenuEvent.OnEventRaise -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaise -= OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    private void OnUnLoadedEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;

        playerStar.gameObject.SetActive(!isMenu);

    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStar.OnHealthChange(persentage);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartbtn);
    }
}
